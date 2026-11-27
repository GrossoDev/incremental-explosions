using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Model;
using UnityEngine;

namespace Shared.View
{
    public class LLMTranslator
    {
        private readonly LLMProvider llmProvider;

        public LLMTranslator() : this(LLMProvider.Instance)
        {
        }

        public LLMTranslator(LLMProvider llmProvider)
        {
            this.llmProvider = llmProvider ?? throw new ArgumentNullException(nameof(llmProvider));
        }

        public async Task<LocalizationTable> TranslateAsync(
            LocalizationTable defaultTable,
            IReadOnlyList<LocalizationKey> keyDefinitions,
            string targetLocale)
        {
            var contextMap = new Dictionary<string, string>();
            foreach (var def in keyDefinitions)
            {
                if (!string.IsNullOrEmpty(def.key))
                    contextMap[def.key] = def.context;
            }

            var promptPayload = new PromptPayload { targetLocale = targetLocale };
            foreach (var kvp in defaultTable.Entries)
            {
                contextMap.TryGetValue(kvp.Key, out string ctx);
                promptPayload.items.Add(new PromptItem
                {
                    key = kvp.Key,
                    text = kvp.Value,
                    context = ctx ?? string.Empty
                });
            }

            var request = new LLMRequest
            {
                SystemInstruction = $"You are a game localization translator. Translate every item's 'text' into locale '{targetLocale}'. " +
                                    "Use each item's 'context' to pick accurate tone, grammar, and terminology. " +
                                    "Preserve keys exactly as given and keep placeholders like {0} or formatting tags intact.",
                UserPrompt = JsonUtility.ToJson(promptPayload),
                ResponseMimeType = "application/json",
                ResponseSchemaJson = JsonUtility.ToJson(new RootSchema())
            };

            try
            {
                var tableData = await llmProvider.GenerateJsonAsync<LocalizationTableData>(request);
                if (tableData == null || tableData.entries == null)
                {
                    Debug.LogError("[LLMTranslator] Failed to deserialize structured output.");
                    return null;
                }

                tableData.locale = targetLocale;
                return new LocalizationTable(targetLocale, tableData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LLMTranslator] {ex.Message}");
                return null;
            }
        }

        [Serializable]
        private class PromptItem
        {
            public string key;
            public string text;
            public string context;
        }

        [Serializable]
        private class PromptPayload
        {
            public string targetLocale;
            public List<PromptItem> items = new();
        }

        [Serializable]
        private class RootSchema
        {
            public string type = "OBJECT";
            public RootProperties properties = new();
            public string[] required = { "locale", "entries" };
        }

        [Serializable]
        private class RootProperties
        {
            public StringSchema locale = new();
            public EntriesArraySchema entries = new();
        }

        [Serializable]
        private class EntriesArraySchema
        {
            public string type = "ARRAY";
            public EntryItemSchema items = new();
        }

        [Serializable]
        private class EntryItemSchema
        {
            public string type = "OBJECT";
            public EntryProperties properties = new();
            public string[] required = { "key", "value" };
        }

        [Serializable]
        private class EntryProperties
        {
            public StringSchema key = new();
            public StringSchema value = new();
        }

        [Serializable]
        private class StringSchema
        {
            public string type = "STRING";
        }
    }
}

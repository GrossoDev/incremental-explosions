using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Model;
using UnityEngine;

namespace Shared.View
{
    public class LLMTranslator
    {
        private const string TranslationPrompt = "You are a game localization translator. Translate every item's 'text' into locale '{0}'. Use each item's 'context' to pick accurate tone, grammar, and terminology. Preserve keys exactly as given and keep placeholders like {{0}} or formatting tags intact.";

        public async Task<LocalizationTable> TranslateAsync(LocalizationTable defaultTable, string targetLocale)
        {
            LLMRequest request = CreateTranslationRequest(defaultTable, targetLocale);

            try
            {
                var table = await LLMProvider.Instance.GenerateJsonAsync<LocalizationTable>(request);
                if (table == null || table.entries == null)
                {
                    Debug.LogError("[LLMTranslator] Failed to deserialize structured output.");
                    return null;
                }

                table.locale = targetLocale;
                return table;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LLMTranslator] {ex.Message}");
                return null;
            }
        }

        private LLMRequest CreateTranslationRequest(LocalizationTable baseTable, string targetLocale)
        {
            var promptPayload = new PromptPayload { targetLocale = targetLocale };
            foreach (var entry in baseTable.entries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.key)) continue;

                promptPayload.items.Add(new PromptItem
                {
                    key = entry.key,
                    text = entry.value,
                    context = entry.context ?? string.Empty
                });
            }

            return new LLMRequest
            {
                SystemInstruction = string.Format(TranslationPrompt, targetLocale),
                UserPrompt = JsonUtility.ToJson(promptPayload),
                ResponseMimeType = "application/json",
                ResponseSchemaJson = JsonUtility.ToJson(new RootSchema())
            };
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Shared.View
{
    public class LLMProvider
    {
        [SerializeField] private string model = "gemini-2.5-flash";
        [SerializeField] private string apiKey;

        private string Endpoint =>
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

        public static LLMProvider Instance { get; } = new LLMProvider();

        public async Task<LocalizationTable> TranslateAsync(
            LocalizationTable defaultTable,
            IReadOnlyList<LocalizationKey> keyDefinitions,
            string targetLocale)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("[LLMTranslator] Gemini API key is missing.");
                return null;
            }

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

            string requestJson = BuildGeminiRequestJson(targetLocale, JsonUtility.ToJson(promptPayload));
            string rawResponse = await PostJsonAsync(Endpoint, requestJson);

            if (string.IsNullOrEmpty(rawResponse))
                return null;

            var geminiResponse = JsonUtility.FromJson<GeminiResponse>(rawResponse);
            string structuredText = geminiResponse?.candidates != null &&
                                    geminiResponse.candidates.Length > 0 &&
                                    geminiResponse.candidates[0].content?.parts != null &&
                                    geminiResponse.candidates[0].content.parts.Length > 0
                ? geminiResponse.candidates[0].content.parts[0].text
                : null;

            if (string.IsNullOrEmpty(structuredText))
            {
                Debug.LogError($"[LLMTranslator] Gemini returned an empty response:\n{rawResponse}");
                return null;
            }

            var tableData = JsonUtility.FromJson<LocalizationTableData>(structuredText);
            if (tableData == null || tableData.entries == null)
            {
                Debug.LogError($"[LLMTranslator] Failed to deserialize structured output:\n{structuredText}");
                return null;
            }

            tableData.locale = targetLocale;
            return new LocalizationTable(targetLocale, tableData);
        }

        private static string BuildGeminiRequestJson(string targetLocale, string itemsJson)
        {
            var request = new GeminiRequest
            {
                systemInstruction = new GeminiContent
                {
                    parts = new[]
                    {
                        new GeminiPart
                        {
                            text = $"You are a game localization translator. Translate every item's 'text' into locale '{targetLocale}'. " +
                                   "Use each item's 'context' to pick accurate tone, grammar, and terminology. " +
                                   "Preserve keys exactly as given and keep placeholders like {0} or formatting tags intact."
                        }
                    }
                },
                contents = new[]
                {
                    new GeminiContent
                    {
                        role = "user",
                        parts = new[] { new GeminiPart { text = itemsJson } }
                    }
                },
                generationConfig = new GeminiGenerationConfig
                {
                    responseMimeType = "application/json",
                    responseSchema = new RootSchema()
                }
            };

            return JsonUtility.ToJson(request);
        }

        private async Task<string> PostJsonAsync(string url, string jsonBody)
        {
            using var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("x-goog-api-key", apiKey);

            var op = req.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[LLMTranslator] Gemini HTTP Error: {req.error}\n{req.downloadHandler.text}");
                return null;
            }

            return req.downloadHandler.text;
        }

        #region Prompt & Gemini Schema DTOs

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
        private class GeminiRequest
        {
            public GeminiContent systemInstruction;
            public GeminiContent[] contents;
            public GeminiGenerationConfig generationConfig;
        }

        [Serializable]
        private class GeminiContent
        {
            public string role;
            public GeminiPart[] parts;
        }

        [Serializable]
        private class GeminiPart
        {
            public string text;
        }

        [Serializable]
        private class GeminiGenerationConfig
        {
            public string responseMimeType;
            public RootSchema responseSchema;
        }

        // Non-recursive schema classes so JsonUtility serializes cleanly without depth warnings
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

        [Serializable]
        private class GeminiResponse
        {
            public Candidate[] candidates;
        }

        [Serializable]
        private class Candidate
        {
            public GeminiContent content;
        }

        #endregion
    }
}
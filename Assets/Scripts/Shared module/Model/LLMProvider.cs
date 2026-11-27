using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Shared.Model
{
    public class LLMRequest
    {
        public string UserPrompt { get; set; }
        public string SystemInstruction { get; set; }
        public string ResponseMimeType { get; set; }
        public string ResponseSchemaJson { get; set; }
        public float? Temperature { get; set; }
        public int? TimeoutSeconds { get; set; }
    }

    public class LLMProvider
    {
        public const string DefaultModel = "gemini-3.5-flash-lite";
        public string APIKEY;

        public static LLMProvider Instance { get; } = new LLMProvider();

        private string apiKey;
        private string model;
        private int defaultTimeoutSeconds;

        public LLMProvider(string apiKey = null, int defaultTimeoutSeconds = 15, string model = DefaultModel)
        {
            this.apiKey = apiKey;
            this.defaultTimeoutSeconds = defaultTimeoutSeconds;
            this.model = string.IsNullOrWhiteSpace(model) ? DefaultModel : model.Trim();
        }

        public void Configure(string apiKey = null, string model = null, int? defaultTimeoutSeconds = null)
        {
            if (apiKey != null)
                this.apiKey = apiKey;

            if (!string.IsNullOrWhiteSpace(model))
                this.model = model.Trim();

            if (defaultTimeoutSeconds.HasValue)
                this.defaultTimeoutSeconds = defaultTimeoutSeconds.Value;
        }

        public async Task<string> GenerateContentAsync(LLMRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.UserPrompt))
                throw new ArgumentException("User prompt cannot be null or empty.", nameof(request));

            string resolvedApiKey = APIKEY;
            if (string.IsNullOrEmpty(resolvedApiKey))
                throw new InvalidOperationException("Gemini API key is not configured.");

            string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";
            string requestJson = BuildRequestJson(request);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestJson);

            using (UnityWebRequest webRequest = new UnityWebRequest(endpoint, UnityWebRequest.kHttpVerbPOST))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("x-goog-api-key", resolvedApiKey);

                int timeout = request.TimeoutSeconds ?? defaultTimeoutSeconds;
                if (timeout > 0)
                    webRequest.timeout = timeout;

                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception($"Gemini API error ({webRequest.responseCode}): {webRequest.error} - {webRequest.downloadHandler.text}");
                }

                return ExtractResponseText(webRequest.downloadHandler.text);
            }
        }

        public async Task<T> GenerateJsonAsync<T>(LLMRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            LLMRequest jsonRequest = new LLMRequest
            {
                UserPrompt = request.UserPrompt,
                SystemInstruction = request.SystemInstruction,
                ResponseMimeType = string.IsNullOrWhiteSpace(request.ResponseMimeType)
                    ? "application/json"
                    : request.ResponseMimeType,
                ResponseSchemaJson = request.ResponseSchemaJson,
                Temperature = request.Temperature,
                TimeoutSeconds = request.TimeoutSeconds
            };

            string jsonText = await GenerateContentAsync(jsonRequest);
            T result = JsonUtility.FromJson<T>(jsonText);
            if (result == null)
                throw new Exception($"Failed to deserialize Gemini JSON response into {typeof(T).Name}.");

            return result;
        }

        public string GetAPIKey()
        {
            return APIKEY;
        }

        public void SetAPIKey(string apiKey)
        {
            this.APIKEY = apiKey;
        }

        private static string BuildRequestJson(LLMRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            bool hasPreviousSection = false;

            if (!string.IsNullOrWhiteSpace(request.SystemInstruction))
            {
                sb.Append("\"systemInstruction\":{\"parts\":[{\"text\":\"");
                sb.Append(EscapeJsonString(request.SystemInstruction));
                sb.Append("\"}]}");
                hasPreviousSection = true;
            }

            if (hasPreviousSection)
                sb.Append(",");

            sb.Append("\"contents\":[{\"role\":\"user\",\"parts\":[{\"text\":\"");
            sb.Append(EscapeJsonString(request.UserPrompt));
            sb.Append("\"}]}]");

            List<string> generationConfigFields = new List<string>();

            if (request.Temperature.HasValue)
            {
                generationConfigFields.Add($"\"temperature\":{request.Temperature.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (!string.IsNullOrWhiteSpace(request.ResponseMimeType))
            {
                generationConfigFields.Add($"\"responseMimeType\":\"{EscapeJsonString(request.ResponseMimeType)}\"");
            }

            if (!string.IsNullOrWhiteSpace(request.ResponseSchemaJson))
            {
                generationConfigFields.Add($"\"responseSchema\":{request.ResponseSchemaJson}");
            }

            if (generationConfigFields.Count > 0)
            {
                sb.Append(",\"generationConfig\":{");
                sb.Append(string.Join(",", generationConfigFields));
                sb.Append("}");
            }

            sb.Append("}");
            return sb.ToString();
        }

        private static string ExtractResponseText(string rawResponse)
        {
            GeminiResponse envelope = JsonUtility.FromJson<GeminiResponse>(rawResponse);
            string text = envelope?.candidates != null
                          && envelope.candidates.Length > 0
                          && envelope.candidates[0].content?.parts != null
                          && envelope.candidates[0].content.parts.Length > 0
                ? envelope.candidates[0].content.parts[0].text
                : null;

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Gemini returned an empty response.");

            string cleaned = text.Trim();
            if (cleaned.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                cleaned = cleaned.Substring(7);
            else if (cleaned.StartsWith("```", StringComparison.Ordinal))
                cleaned = cleaned.Substring(3);

            if (cleaned.EndsWith("```", StringComparison.Ordinal))
                cleaned = cleaned.Substring(0, cleaned.Length - 3);

            cleaned = cleaned.Trim();
            if (string.IsNullOrWhiteSpace(cleaned))
                throw new Exception("Gemini returned an empty response.");

            return cleaned;
        }

        private static string EscapeJsonString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            StringBuilder sb = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    default:
                        if (c < ' ')
                            sb.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)c);
                        else
                            sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }

        [Serializable]
        private class GeminiResponse
        {
            public GeminiCandidate[] candidates;
        }

        [Serializable]
        private class GeminiCandidate
        {
            public GeminiContent content;
        }

        [Serializable]
        private class GeminiContent
        {
            public GeminiPart[] parts;
        }

        [Serializable]
        private class GeminiPart
        {
            public string text;
        }
    }
}

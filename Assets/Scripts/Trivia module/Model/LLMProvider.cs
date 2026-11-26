using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Trivia.Model
{
    public class LLMProvider : IQuestionProvider
    {
        private const string DefaultModelName = "gemini-3.5-flash-lite";
        private readonly string apiURL;
        private readonly string apiKey;
        private readonly int timeoutSeconds;
        private static readonly List<string> recentQuestions = new List<string>();

        public LLMProvider(string apiKey, int timeoutSeconds, string modelName = DefaultModelName)
        {
            string resolvedModel = string.IsNullOrWhiteSpace(modelName) ? DefaultModelName : modelName.Trim();
            this.apiURL = $"https://generativelanguage.googleapis.com/v1beta/models/{resolvedModel}:generateContent";
            this.apiKey = apiKey;
            this.timeoutSeconds = timeoutSeconds;
        }

        public async Task<TriviaQuestion> GetQuestionAsync(int difficulty)
        {
            try
            {
                if (string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException("API Key no configurada.");

                string prompt = BuildQuestionPrompt(difficulty);
                string requestJson = BuildRequestPayload(prompt);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(requestJson);

                using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
                {
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("x-goog-api-key", apiKey);
                    request.timeout = timeoutSeconds;

                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        // Incluir downloadHandler.text ayuda muchísimo a debuggear errores 400/403/429
                        throw new Exception($"Error API ({request.responseCode}): {request.error} - {request.downloadHandler.text}");
                    }

                    TriviaQuestion question = ParseGeminiResponse(request.downloadHandler.text);
                    RegisterRecentQuestion(question.GetStatement());
                    return question;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LLMProvider] Gemini request failed, falling back to local trivia: {ex.Message}");
                throw;
            }
        }

        private string BuildQuestionPrompt(int difficulty)
        {
            string history = recentQuestions.Count > 0
                ? $" Evita repetir estos temas o preguntas ya hechas: {string.Join(" | ", recentQuestions)}."
                : "";

            return $"Genera una pregunta de trivia original de cultura general Argentina (historia, geografía, música, deportes, costumbres, cine o literatura) " +
                   $"con dificultad {difficulty} en una escala de 1 (muy fácil) a 50 (muy difícil), con exactamente 4 opciones y correctOptionIndex entre 0 y 3.{history}";
        }

        private string BuildRequestPayload(string prompt)
        {
            string escapedPrompt = prompt
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");

            return $@"{{
                ""contents"": [{{
                    ""parts"": [{{ ""text"": ""{escapedPrompt}"" }}]
                }}],
                ""generationConfig"": {{
                    ""temperature"": 0.9,
                    ""responseMimeType"": ""application/json"",
                    ""responseSchema"": {{
                        ""type"": ""OBJECT"",
                        ""properties"": {{
                            ""statement"": {{ ""type"": ""STRING"" }},
                            ""options"": {{
                                ""type"": ""ARRAY"",
                                ""minItems"": 4,
                                ""maxItems"": 4,
                                ""items"": {{ ""type"": ""STRING"" }}
                            }},
                            ""correctOptionIndex"": {{ ""type"": ""INTEGER"" }}
                        }},
                        ""required"": [""statement"", ""options"", ""correctOptionIndex""]
                    }}
                }}
            }}";
        }

        private void RegisterRecentQuestion(string statement)
        {
            recentQuestions.Add(statement);
            if (recentQuestions.Count > 15) // Mantiene memoria de las últimas 15 preguntas
                recentQuestions.RemoveAt(0);
        }

        private TriviaQuestion ParseGeminiResponse(string rawJson)
        {
            GeminiResponse envelope = JsonUtility.FromJson<GeminiResponse>(rawJson);
            if (envelope?.candidates == null || envelope.candidates.Length == 0)
                throw new Exception("Respuesta vacía de Gemini.");

            string questionJson = envelope.candidates[0].content.parts[0].text.Trim();
            TriviaQuestionDTO dto = JsonUtility.FromJson<TriviaQuestionDTO>(questionJson);

            if (dto == null
                || string.IsNullOrWhiteSpace(dto.statement)
                || dto.options == null
                || dto.options.Count != 4
                || dto.correctOptionIndex < 0
                || dto.correctOptionIndex >= dto.options.Count)
            {
                throw new Exception("La API devolvió un formato de pregunta inválido.");
            }

            return new TriviaQuestion(dto.statement, dto.options, dto.correctOptionIndex);
        }

        [Serializable] private class GeminiResponse { public Candidate[] candidates; }
        [Serializable] private class Candidate { public Content content; }
        [Serializable] private class Content { public Part[] parts; }
        [Serializable] private class Part { public string text; }

        [Serializable]
        private class TriviaQuestionDTO
        {
            public string statement;
            public List<string> options;
            public int correctOptionIndex;
        }
    }
}

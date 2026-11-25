using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LLMProvider : IQuestionProvider
{
    private readonly string apiURL = "[https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent](https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent)";
    private readonly string apiKey;
    private readonly int timeoutSeconds;

    public LLMProvider(string apiKey, int timeoutSeconds = 5)
    {
        this.apiKey = apiKey;
        this.timeoutSeconds = timeoutSeconds;
    }

    public async Task<TriviaQuestion> GetQuestionAsync(int difficulty)
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
                throw new Exception($"Error de red o timeout en API: {request.error}");
            }

            return ParseGeminiResponse(request.downloadHandler.text);
        }
    }

    private string BuildQuestionPrompt(int difficulty)
    {
        return $"Genera una pregunta de trivia de opción múltiple de cultura general Argentina con dificultad {difficulty} en una escala de 1 (muy fácil) a 50 (muy difícil). " +
               "Devuelve exclusivamente un objeto JSON con esta estructura exacta: " +
               "{\"statement\": \"texto de la pregunta\", \"options\": [\"opcion 0\", \"opcion 1\", \"opcion 2\", \"opcion 3\"], \"correctOptionIndex\": 0}";
    }

    private string BuildRequestPayload(string prompt)
    {
        string escapedPrompt = prompt.Replace("\\", "\\\\").Replace("\"", "\\\"");
        return $@"{{
            ""contents"": [{{
                ""parts"": [{{ ""text"": ""{escapedPrompt}"" }}]
            }}],
            ""generationConfig"": {{
                ""responseMimeType"": ""application/json""
            }}
        }}";
    }

    private TriviaQuestion ParseGeminiResponse(string rawJson)
    {
        GeminiResponse envelope = JsonUtility.FromJson<GeminiResponse>(rawJson);
        if (envelope?.candidates == null || envelope.candidates.Length == 0)
            throw new Exception("Respuesta vacía de Gemini.");

        string questionJson = envelope.candidates[0].content.parts[0].text
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        TriviaQuestionDTO dto = JsonUtility.FromJson<TriviaQuestionDTO>(questionJson);

        if (dto == null || dto.options == null || dto.options.Count < 2)
            throw new Exception("La API devolvió un formato de pregunta inválido.");

        return new TriviaQuestion(dto.statement, dto.options, dto.correctOptionIndex);
    }

    [Serializable]
    private class GeminiResponse { public Candidate[] candidates; }

    [Serializable]
    private class Candidate { public Content content; }

    [Serializable]
    private class Content { public Part[] parts; }

    [Serializable]
    private class Part { public string text; }

    [Serializable]
    private class TriviaQuestionDTO
    {
        public string statement;
        public List<string> options;
        public int correctOptionIndex;
    }
}
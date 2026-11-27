using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Model;
using UnityEngine;

namespace Trivia.Model
{
    public class LLMTriviaProvider : IQuestionProvider
    {
        private const string TriviaResponseSchemaJson = @"{
            ""type"": ""OBJECT"",
            ""properties"": {
                ""statement"": { ""type"": ""STRING"" },
                ""options"": {
                    ""type"": ""ARRAY"",
                    ""minItems"": 4,
                    ""maxItems"": 4,
                    ""items"": { ""type"": ""STRING"" }
                },
                ""correctOptionIndex"": { ""type"": ""INTEGER"" }
            },
            ""required"": [""statement"", ""options"", ""correctOptionIndex""]
        }";

        private static readonly List<string> recentQuestions = new List<string>();
        private readonly LLMProvider llmProvider;
        private readonly int? timeoutSeconds;

        public LLMTriviaProvider(LLMProvider llmProvider, int? timeoutSeconds = null)
        {
            this.llmProvider = llmProvider ?? throw new ArgumentNullException(nameof(llmProvider));
            this.timeoutSeconds = timeoutSeconds;
        }

        public async Task<TriviaQuestion> GetQuestionAsync(int difficulty)
        {
            try
            {
                LLMRequest request = new LLMRequest
                {
                    UserPrompt = BuildQuestionPrompt(difficulty),
                    Temperature = 0.9f,
                    ResponseMimeType = "application/json",
                    ResponseSchemaJson = TriviaResponseSchemaJson,
                    TimeoutSeconds = timeoutSeconds
                };

                TriviaQuestionDTO dto = await llmProvider.GenerateJsonAsync<TriviaQuestionDTO>(request);

                if (dto == null
                    || string.IsNullOrWhiteSpace(dto.statement)
                    || dto.options == null
                    || dto.options.Count != 4
                    || dto.correctOptionIndex < 0
                    || dto.correctOptionIndex >= dto.options.Count)
                {
                    throw new Exception("La API devolvió un formato de pregunta inválido.");
                }

                TriviaQuestion question = new TriviaQuestion(dto.statement, dto.options, dto.correctOptionIndex);
                RegisterRecentQuestion(question.GetStatement());
                return question;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LLMTriviaProvider] Gemini request failed, falling back to local trivia: {ex.Message}");
                throw;
            }
        }

        private static string BuildQuestionPrompt(int difficulty)
        {
            string history = recentQuestions.Count > 0
                ? $" Evita repetir estos temas o preguntas ya hechas: {string.Join(" | ", recentQuestions)}."
                : "";

            return $"Genera una pregunta de trivia original de cultura general Argentina (historia, geografía, música, deportes, costumbres, cine o literatura) " +
                   $"con dificultad {difficulty} en una escala de 1 (muy fácil) a 50 (muy difícil), con exactamente 4 opciones y correctOptionIndex entre 0 y 3.{history}";
        }

        private static void RegisterRecentQuestion(string statement)
        {
            recentQuestions.Add(statement);
            if (recentQuestions.Count > 15)
                recentQuestions.RemoveAt(0);
        }

        [Serializable]
        private class TriviaQuestionDTO
        {
            public string statement;
            public List<string> options;
            public int correctOptionIndex;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Trivia.Model
{
    public class LocalTriviaProvider : IQuestionProvider
    {
        private readonly string filePath;
        private readonly Dictionary<int, List<TriviaQuestion>> questionsByDifficulty;

        public LocalTriviaProvider(string filePath)
        {
            this.filePath = filePath;
            questionsByDifficulty = new Dictionary<int, List<TriviaQuestion>>();
            LoadQuestionsFromJson();
        }

        public Task<TriviaQuestion> GetQuestionAsync(int difficulty)
        {
            if (questionsByDifficulty.Count == 0)
                throw new InvalidOperationException("El banco local de preguntas está vacío o no pudo cargarse.");

            int targetDifficulty = questionsByDifficulty.ContainsKey(difficulty)
                ? difficulty
                : questionsByDifficulty.Keys.OrderBy(d => Math.Abs(d - difficulty)).First();

            List<TriviaQuestion> candidates = questionsByDifficulty[targetDifficulty];
            int randomIndex = UnityEngine.Random.Range(0, candidates.Count);

            return Task.FromResult(candidates[randomIndex]);
        }

        private void LoadQuestionsFromJson()
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Debug.LogError("La ruta del archivo JSON de trivia local está vacía.");
                return;
            }

            TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

            if (jsonFile == null)
            {
                Debug.LogError($"No se encontró el archivo JSON en Resources/{filePath}.json");
                return;
            }

            TriviaBankDTO bank = JsonUtility.FromJson<TriviaBankDTO>(jsonFile.text);
            if (bank == null || bank.questions == null)
                return;

            foreach (var dto in bank.questions)
            {
                if (dto == null
                    || string.IsNullOrWhiteSpace(dto.statement)
                    || dto.options == null
                    || dto.options.Count != 4
                    || dto.correctOptionIndex < 0
                    || dto.correctOptionIndex >= dto.options.Count)
                {
                    continue;
                }

                if (!questionsByDifficulty.ContainsKey(dto.difficulty))
                {
                    questionsByDifficulty[dto.difficulty] = new List<TriviaQuestion>();
                }

                questionsByDifficulty[dto.difficulty].Add(
                    new TriviaQuestion(dto.statement, dto.options, dto.correctOptionIndex)
                );
            }
        }

        [Serializable]
        private class TriviaBankDTO { public List<LocalQuestionDTO> questions; }

        [Serializable]
        private class LocalQuestionDTO
        {
            public string statement;
            public List<string> options;
            public int correctOptionIndex;
            public int difficulty;
        }
    }
}

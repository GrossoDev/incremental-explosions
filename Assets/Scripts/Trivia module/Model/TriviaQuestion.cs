using System;
using System.Collections.Generic;

namespace Trivia.Model
{
    public class TriviaQuestion
    {
        private readonly string statement;
        private readonly List<string> options;
        private readonly int correctOptionIndex;

        public TriviaQuestion(string statement, List<string> options, int correctOptionIndex)
        {
            if (string.IsNullOrWhiteSpace(statement))
                throw new ArgumentException("Question statement cannot be null or empty.", nameof(statement));

            if (options == null || options.Count == 0)
                throw new ArgumentException("Question options cannot be null or empty.", nameof(options));

            if (correctOptionIndex < 0 || correctOptionIndex >= options.Count)
                throw new ArgumentOutOfRangeException(nameof(correctOptionIndex), "Correct option index is out of range.");

            this.statement = statement;
            this.options = new List<string>(options);
            this.correctOptionIndex = correctOptionIndex;
        }

        public string GetStatement() => statement;

        public IReadOnlyList<string> GetOptions() => options;

        public bool IsCorrectAnswer(int optionIndex) => optionIndex == correctOptionIndex;
    }
}

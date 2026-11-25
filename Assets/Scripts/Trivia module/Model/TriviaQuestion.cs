using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TriviaQuestion
{
    private string statement;
    private List<string> options;
    private int correctOptionIndex;

    public TriviaQuestion(string statement, List<string> options, int correctOptionIndex)
    {
        this.statement = statement;
        this.options = options;
        this.correctOptionIndex = correctOptionIndex;
    }

    public string GetStatement() => statement;

    public List<string> GetOptions() => options;

    public bool IsCorrectAnswer(int optionIndex) => optionIndex == correctOptionIndex;
}

using System;
using System.Threading.Tasks;
using Shared.Model;

public class TriviaSession
{
    private int roundEarnings;
    private readonly int difficulty;
    private TriviaQuestion currentQuestion;
    private readonly IQuestionProvider primaryProvider;
    private readonly IQuestionProvider fallbackProvider;
    private bool wasCorrect;
    private bool isResolved;

    public TriviaSession(int roundEarnings, int difficulty, IQuestionProvider primaryProvider, IQuestionProvider fallbackProvider)
    {
        this.roundEarnings = roundEarnings;
        this.difficulty = difficulty;
        this.primaryProvider = primaryProvider;
        this.fallbackProvider = fallbackProvider;
        this.isResolved = false;
    }

    public int GetRoundEarnings() => roundEarnings;

    public bool WasCorrect() => wasCorrect;

    public async Task<TriviaQuestion> GetQuestionAsync()
    {
        try
        {
            currentQuestion = await primaryProvider.GetQuestionAsync(difficulty);
        }
        catch (Exception)
        {
            currentQuestion = await fallbackProvider.GetQuestionAsync(difficulty);
        }
        return currentQuestion;
    }

    public void Answer(int option)
    {
        if (isResolved || currentQuestion == null) return;

        wasCorrect = currentQuestion.IsCorrectAnswer(option);
        roundEarnings = wasCorrect ? roundEarnings * 2 : roundEarnings / 2;

        isResolved = true;
        UpdateCoins();
    }

    public void Refrain()
    {
        if (isResolved) return;

        isResolved = true;
        UpdateCoins();
    }

    private void UpdateCoins()
    {
        Game.Instance.SetTotalCoins(Game.Instance.GetTotalCoins() + roundEarnings);
    }
}
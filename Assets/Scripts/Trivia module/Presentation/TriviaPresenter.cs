
using System;
using System.Threading.Tasks;

public class TriviaPresenter
{
    private readonly ITriviaView view;
    private readonly TriviaSession triviaSession;
    private readonly Action onTriviaCompleted;

    public TriviaPresenter(ITriviaView view, TriviaSession triviaSession, Action onTriviaCompleted = null)
    {
        this.view = view;
        this.triviaSession = triviaSession;
        this.onTriviaCompleted = onTriviaCompleted;
    }

    public void StartTrivia()
    {
        view.ShowProposal(triviaSession.GetRoundEarnings());
    }

    public async void OnAcceptTrivia()
    {
        try
        {
            TriviaQuestion question = await triviaSession.GetQuestionAsync();
            view.ShowQuestion(question.GetStatement(), question.GetOptions());
        }
        catch (Exception)
        {
            OnRejectTrivia();
        }
    }

    public void OnRejectTrivia()
    {
        triviaSession.Refrain();
        onTriviaCompleted?.Invoke();
    }

    public void OnSelectedOption(int option)
    {
        triviaSession.Answer(option);
        view.ShowResult(triviaSession.WasCorrect(), triviaSession.GetRoundEarnings());
    }

    public void OnContinueAfterResult()
    {
        onTriviaCompleted?.Invoke();
    }
}

using System;
using Trivia.Model;
using Trivia.View;

namespace Trivia.Presentation
{
    public class TriviaPresenter
    {
        private readonly TriviaView view;
        private readonly TriviaSession triviaSession;
        private bool isLoadingQuestion;

        public TriviaPresenter(TriviaView view, TriviaSession triviaSession)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.triviaSession = triviaSession ?? throw new ArgumentNullException(nameof(triviaSession));
        }

        public void StartTrivia()
        {
            view.ShowProposal(triviaSession.GetRoundEarnings());
        }

        public async void OnAcceptTrivia()
        {
            if (isLoadingQuestion || triviaSession.IsResolved())
                return;

            isLoadingQuestion = true;
            view.ShowLoading();

            try
            {
                TriviaQuestion question = await triviaSession.GetQuestionAsync();

                if (triviaSession.IsResolved())
                    return;

                view.ShowQuestion(question.GetStatement(), question.GetOptions());
            }
            catch (Exception)
            {
                OnRejectTrivia();
            }
            finally
            {
                isLoadingQuestion = false;
            }
        }

        public void OnRejectTrivia()
        {
            if (triviaSession.IsResolved())
                return;

            triviaSession.Refrain();
            view.NavigateToNextScene();
        }

        public void OnSelectedOption(int optionIndex)
        {
            if (isLoadingQuestion || triviaSession.IsResolved())
                return;

            triviaSession.Answer(optionIndex);
            view.ShowResult(triviaSession.WasCorrect(), triviaSession.GetRoundEarnings());
        }

        public void OnContinueAfterResult()
        {
            if (!triviaSession.IsResolved())
                return;

            view.NavigateToNextScene();
        }
    }
}

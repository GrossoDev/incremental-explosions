using System;
using System.Collections.Generic;
using Shared.View;
using TMPro;
using Trivia.Model;
using Trivia.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Trivia.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class TriviaView : MonoBehaviour
    {
        [Header("Main Texts")]
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private Text coinsText;

        [Header("Answer Options")]
        [SerializeField] private GameObject optionsContainer;
        [SerializeField] private List<Button> optionButtons;
        [SerializeField] private List<TextMeshProUGUI> optionTexts;

        [Header("Footer Buttons")]
        [SerializeField] private Button answerButton;
        [SerializeField] private Button refrainButton;
        [SerializeField] private Button continueButton;

        [Header("Trivia Providers")]
        [SerializeField] private string geminiApiKey = "";
        [SerializeField] private int requestTimeoutSeconds = 5;
        [SerializeField] private string localQuestionsResourcePath = "TriviaQuestions";

        private SceneSwitcher sceneSwitcher;
        private TriviaPresenter presenter;

        private void Start()
        {
            sceneSwitcher = GetComponent<SceneSwitcher>();

            IQuestionProvider primaryProvider = new Trivia.Model.LLMProvider(geminiApiKey, requestTimeoutSeconds);
            IQuestionProvider fallbackProvider = new LocalTriviaProvider(localQuestionsResourcePath);
            TriviaSession session = new TriviaSession(primaryProvider, fallbackProvider);

            presenter = new TriviaPresenter(this, session);
            presenter.StartTrivia();
        }

        public void OnAcceptButtonClicked()
        {
            presenter.OnAcceptTrivia();
        }

        public void OnRefrainButtonClicked()
        {
            presenter.OnRejectTrivia();
        }

        public void OnOptionButtonClicked(int optionIndex)
        {
            presenter.OnSelectedOption(optionIndex);
        }

        public void OnContinueButtonClicked()
        {
            presenter.OnContinueAfterResult();
        }

        public void ShowProposal(int collectedCoins)
        {
            if (coinsText != null)
                coinsText.text = $"Round earnings: ${collectedCoins}";

            if (mainText != null)
                mainText.text = "Do you want to risk your round earnings on a trivia question?\n(Correct: x2 | Incorrect: /2)";

            if (optionsContainer != null)
                optionsContainer.SetActive(false);

            SetButtonState(answerButton, true, true);
            SetButtonState(refrainButton, true, true);
            SetButtonState(continueButton, false, false);
        }

        public void ShowLoading()
        {
            if (mainText != null)
                mainText.text = "Loading question...";

            if (optionsContainer != null)
                optionsContainer.SetActive(false);

            SetButtonState(answerButton, true, false);
            SetButtonState(refrainButton, true, false);
            SetButtonState(continueButton, false, false);
        }

        public void ShowQuestion(string question, IReadOnlyList<string> options)
        {
            if (mainText != null)
                mainText.text = question;

            if (optionsContainer != null)
                optionsContainer.SetActive(true);

            if (optionButtons != null)
            {
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    bool hasOption = options != null && i < options.Count;
                    SetButtonState(optionButtons[i], hasOption, hasOption);

                    if (hasOption && optionTexts != null && i < optionTexts.Count && optionTexts[i] != null)
                    {
                        optionTexts[i].text = options[i];
                    }
                }
            }

            SetButtonState(answerButton, false, false);
            SetButtonState(refrainButton, false, false);
            SetButtonState(continueButton, false, false);
        }

        public void ShowResult(bool correctAnswer, int finalCoins)
        {
            if (optionsContainer != null)
                optionsContainer.SetActive(false);

            if (mainText != null)
                mainText.text = correctAnswer
                    ? "Correct answer! Your round earnings doubled!"
                    : "Wrong answer! Your round earnings were halved.";

            if (coinsText != null)
                coinsText.text = $"Final round earnings: ${finalCoins}";

            SetButtonState(answerButton, false, false);
            SetButtonState(refrainButton, false, false);
            SetButtonState(continueButton, true, true);
        }

        public void NavigateToNextScene()
        {
            Time.timeScale = 1f;
            sceneSwitcher.SwitchToNext();
        }

        private void SetButtonState(Button button, bool active, bool interactable)
        {
            if (button == null)
                return;

            button.gameObject.SetActive(active);
            button.interactable = interactable;
        }
    }
}

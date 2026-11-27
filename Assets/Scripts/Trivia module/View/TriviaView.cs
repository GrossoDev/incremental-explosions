using System;
using System.Collections.Generic;
using Shared.Model;
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
        [SerializeField] private TMP_Text coinsTextComponent;
        [SerializeField] private TMP_Text resultTextComponent;

        [Header("Panels")]
        [SerializeField] private GameObject proposalContainer;
        [SerializeField] private GameObject loadingContainer;
        [SerializeField] private GameObject questionContainer;
        [SerializeField] private GameObject resultContainer;

        [Header("Question elements")]
        [SerializeField] private TMP_Text questionTextComponent;
        [SerializeField] private List<Button> optionButtons;
        [SerializeField] private List<TMP_Text> optionTextComponents;

        [Header("Strings")]
        [SerializeField] private LocalizedText coinsText;
        [SerializeField] private LocalizedText finalCoinsText;
        [SerializeField] private LocalizedText rightAnswerText;
        [SerializeField] private LocalizedText wrongAnswerText;

        [Header("Trivia Providers")]
        [SerializeField] private string geminiApiKey = "";
        [SerializeField] private int requestTimeoutSeconds = 5;
        [SerializeField] private string localQuestionsResourcePath = "TriviaQuestions";

        private SceneSwitcher sceneSwitcher;
        private TriviaPresenter presenter;

        private void Start()
        {
            sceneSwitcher = GetComponent<SceneSwitcher>();

            LLMProvider llmProvider = string.IsNullOrWhiteSpace(geminiApiKey)
                ? LLMProvider.Instance
                : new LLMProvider(geminiApiKey.Trim(), requestTimeoutSeconds);

            IQuestionProvider primaryProvider = new LLMTriviaProvider(llmProvider, requestTimeoutSeconds);
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
            coinsTextComponent.text = coinsText.GetFormatted(collectedCoins);
            proposalContainer.SetActive(true);
        }

        public void ShowLoading()
        {
            proposalContainer.SetActive(false);
            loadingContainer.SetActive(true);
        }

        public void ShowQuestion(string question, IReadOnlyList<string> options)
        {
            questionTextComponent.text = question;
            loadingContainer.SetActive(false);
            questionContainer.SetActive(true);

            if (optionButtons != null)
            {
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    bool hasOption = options != null && i < options.Count;

                    if (hasOption && optionTextComponents != null && i < optionTextComponents.Count && optionTextComponents[i] != null)
                    {
                        optionTextComponents[i].text = options[i];
                    }
                }
            }
        }

        public void ShowResult(bool correctAnswer, int finalCoins)
        {
            questionContainer.SetActive(false);
            resultContainer.SetActive(true);

            resultTextComponent.text = correctAnswer
                ? rightAnswerText.Get()
                : wrongAnswerText.Get();

            coinsTextComponent.text = finalCoinsText.GetFormatted(finalCoins);
        }

        public void NavigateToNextScene()
        {
            Time.timeScale = 1f;
            sceneSwitcher.SwitchToNext();
        }
    }
}

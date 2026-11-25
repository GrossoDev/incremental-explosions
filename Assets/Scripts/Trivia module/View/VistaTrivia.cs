using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriviaView : MonoBehaviour
{
    [Header("Main Texts")]
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Answer Options")]
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private List<TextMeshProUGUI> optionTexts;

    [Header("Footer Buttons")]
    [SerializeField] private Button answerButton;
    [SerializeField] private Button refrainButton;

    private TriviaPresenter presenter;

    public void Initialize(TriviaPresenter presenter)
    {
        this.presenter = presenter;
    }

    // --- Métodos públicos para asignar en On Click () desde el Inspector ---

    public void OnAcceptButtonClicked()
    {
        answerButton.interactable = false;
        refrainButton.interactable = false;
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

    // --- Métodos llamados por TriviaPresenter ---

    public void ShowProposal(int collectedCoins)
    {
        coinsText.text = $"Round earnings: ${collectedCoins}";
        mainText.text = "Do you want to risk your round earnings on a trivia question?\n(Correct: x2 | Incorrect: /2)";

        optionsContainer.SetActive(false);

        answerButton.gameObject.SetActive(true);
        answerButton.interactable = true;

        refrainButton.gameObject.SetActive(true);
        refrainButton.interactable = true;

    }

    public void ShowQuestion(string question, IReadOnlyList<string> options)
    {
        mainText.text = question;
        optionsContainer.SetActive(true);

        for (int i = 0; i < optionButtons.Count; i++)
        {
            bool hasOption = i < options.Count;
            optionButtons[i].gameObject.SetActive(hasOption);
            if (hasOption)
            {
                optionTexts[i].text = options[i];
            }
        }

        answerButton.gameObject.SetActive(false);
        refrainButton.gameObject.SetActive(false);
    }

    public void ShowResult(bool correctAnswer, int finalCoins)
    {
        optionsContainer.SetActive(false);

        mainText.text = correctAnswer ? "Correct answer!" : "Wrong answer!";
        coinsText.text = $"Final earnings: ${finalCoins}";

        answerButton.gameObject.SetActive(false);
        refrainButton.gameObject.SetActive(false);
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VistaTrivia : MonoBehaviour, ITriviaView
{
    [Header("Textos Principales")]
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Opciones de Respuesta")]
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private List<TextMeshProUGUI> optionTexts;

    [Header("Botones del Footer")]
    [SerializeField] private Button answerButton;
    [SerializeField] private Button refrainButton;
    [SerializeField] private Button continueButton;

    private TriviaPresenter presenter;

    public void Initialize(TriviaPresenter presenter)
    {
        this.presenter = presenter;
        BindButtons();
    }

    private void BindButtons()
    {
        answerButton.onClick.RemoveAllListeners();
        answerButton.onClick.AddListener(() =>
        {
            answerButton.interactable = false;
            refrainButton.interactable = false;
            presenter.OnAcceptTrivia();
        });

        refrainButton.onClick.RemoveAllListeners();
        refrainButton.onClick.AddListener(() => presenter.OnRejectTrivia());

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => presenter.OnContinueAfterResult());

        for (int i = 0; i < optionButtons.Count; i++)
        {
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => presenter.OnSelectedOption(index));
        }
    }

    public void ShowProposal(int collectedCoins)
    {
        coinsText.text = $"Ganancias de ronda: ${collectedCoins}";
        mainText.text = "¿Querés arriesgar tus ganancias de la ronda en una trivia?\n(Correcta: x2 | Incorrecta: /2)";

        optionsContainer.SetActive(false);

        answerButton.gameObject.SetActive(true);
        answerButton.interactable = true;

        refrainButton.gameObject.SetActive(true);
        refrainButton.interactable = true;

        continueButton.gameObject.SetActive(false);
    }

    public void ShowQuestion(string question, List<string> options)
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
        continueButton.gameObject.SetActive(false);
    }

    public void ShowResult(bool correctAnswer, int finalCoins)
    {
        optionsContainer.SetActive(false);

        mainText.text = correctAnswer ? "¡Respuesta correcta!" : "Respuesta incorrecta";
        coinsText.text = $"Ganancias finales: ${finalCoins}";

        answerButton.gameObject.SetActive(false);
        refrainButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }
}
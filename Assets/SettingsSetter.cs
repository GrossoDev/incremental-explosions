using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField apiKey;
    [SerializeField] private TMP_InputField idioma;
    
    public void SetAPIKey()
    {
        if (string.IsNullOrWhiteSpace(apiKey.text))
        {
            Debug.LogError("API Key cannot be empty.");
            return;
        }

        Shared.Model.LLMProvider.Instance.SetAPIKey(apiKey.text);
        Debug.Log("API Key set successfully.");
    }

    public async void SetIdioma()
    {
        if (string.IsNullOrWhiteSpace(idioma.text))
        {
            Debug.LogError("Idioma cannot be empty.");
            return;
        }

       await Shared.View.Localization.Instance.SetLanguageAsync(idioma.text);
        Debug.Log("Idioma set successfully.");
    }
}

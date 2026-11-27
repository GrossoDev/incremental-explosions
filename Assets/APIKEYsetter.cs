using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class APIKEYsetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField apiKey;
    
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
}

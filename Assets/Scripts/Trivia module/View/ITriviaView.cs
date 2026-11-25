using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public interface ITriviaView
{
    void ShowProposal(int collectedCoins);
    void ShowQuestion(string question, List<string> options);
    void ShowResult(bool correctAnswer, int finalCoins);
}
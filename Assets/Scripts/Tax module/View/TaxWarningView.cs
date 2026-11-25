using Shared.View;
using Tax.Presentation;
using TMPro;
using UnityEngine;

namespace Tax.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class TaxWarningView : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI amountText;

        [Header("Config")]
        [SerializeField] private TaxView taxView;

        private SceneSwitcher sceneSwitcher;

        private void OnEnable()
        {
            sceneSwitcher = GetComponent<SceneSwitcher>();
            UpdateTexts();
        }

        public void OnContinue()
        {
            sceneSwitcher.SwitchToNext();
        }

        private void UpdateTexts()
        {
            TaxConfig nextTax = taxView.GetNextTax();

            int currentLevel = taxView.GetCurrentLevel();
            int levels = nextTax.level - currentLevel;
            int amount = nextTax.tax;

            if (levels > 1)
                levelText.text = $"{levels} more levels until the next tax";
            else
                levelText.text = "Next level you'll have to pay taxes";

            amountText.text = $"You'll need to pay ${amount}";
        }
    }
}
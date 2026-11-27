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
        [SerializeField] private TMP_Text levelTextComponent;
        [SerializeField] private TMP_Text amountTextComponent;

        [Header("Strings")]
        [SerializeField] private LocalizedText levelGreaterThanOneText;
        [SerializeField] private LocalizedText levelEqualToOneText;
        [SerializeField] private LocalizedText amountText;

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
            TaxConfig nextTax = taxView.GetPresenter().GetNextTax();

            int currentLevel = taxView.GetPresenter().GetCurrentLevel();
            int levels = nextTax.level - currentLevel;
            int amount = nextTax.tax;

            if (levels > 1)
                levelTextComponent.text = levelGreaterThanOneText.GetFormatted(levels);
            else
                levelTextComponent.text = levelEqualToOneText.Get();

            amountTextComponent.text = amountText.GetFormatted(amount);
        }
    }
}
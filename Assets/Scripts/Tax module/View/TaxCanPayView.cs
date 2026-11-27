using Shared.View;
using Tax.Presentation;
using TMPro;
using UnityEngine;

namespace Tax.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class TaxCanPayView : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private TMP_Text amountTextComponent;

        [Header("Strings")]
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
            taxView.GetPresenter().PayTax();
            sceneSwitcher.SwitchToNext();
        }

        private void UpdateTexts()
        {
            TaxConfig tax = taxView.GetPresenter().GetTax();
            int amount = tax.tax;

            amountTextComponent.text = amountText.GetFormatted(amount);
        }
    }
}
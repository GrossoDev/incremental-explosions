using Shared.View;
using Tax.Presentation;
using TMPro;
using UnityEngine;

namespace Tax.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class TaxCannotPayView : MonoBehaviour
    {
        [Header("UI elements")]
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
            TaxConfig tax = taxView.GetPresenter().GetTax();
            int amount = tax.tax;

            amountText.text = $"You don't have enough coins to pay ${amount}";
        }
    }
}
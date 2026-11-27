using Shared.View;
using Shop.Presentation;
using TMPro;
using UnityEngine;

namespace Shop.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsTextComponent;

        [Header("Strings")]
        [SerializeField] private LocalizedText pluralAmountText;
        [SerializeField] private LocalizedText singularAmountText;
        [SerializeField] private LocalizedText zeroAmountText;

        private SceneSwitcher sceneSwitcher;
        private ShopPresenter presenter;

        private void Start()
        {
            sceneSwitcher = GetComponent<SceneSwitcher>();
            presenter = new ShopPresenter();
            sceneSwitcher = GetComponent<SceneSwitcher>();

            UpdateTexts();
            presenter.AdvanceLevel();
        }

        private void LateUpdate()
        {
            UpdateTexts();
        }

        public ShopPresenter GetPresenter()
        {
            return presenter;
        }

        public void OnContinue()
        {
            sceneSwitcher.SwitchToNext();
        }

        private void UpdateTexts()
        {
            int coins = presenter.GetCoins();

            if (coins > 1)
                coinsTextComponent.text = pluralAmountText.GetFormatted(coins);
            else if (coins == 1)
                coinsTextComponent.text = singularAmountText.Get();
            else
                coinsTextComponent.text = zeroAmountText.Get();
        }

        public void OnSave()
        {
            GameView.Instance.SaveGame();
        }
    }
}
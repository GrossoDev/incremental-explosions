using Shared.View;
using Shop.Presentation;
using TMPro;
using UnityEngine;

namespace Shop.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

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
            presenter.AdvanceLevel();
            sceneSwitcher.SwitchToNext();
        }

        private void UpdateTexts()
        {
            int coins = presenter.GetCoins();

            if (coins > 1)
                coinsText.text = $"You have {coins} coins";
            else if (coins == 1)
                coinsText.text = "You have 1 coin";
            else
                coinsText.text = "You are broke";
        }

        public void OnSave()
        {
            GameView.Instance.SaveGame();
        }
    }
}
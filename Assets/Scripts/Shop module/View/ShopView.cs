using Shared.View;
using Shop.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Text coinsText;

        private ShopPresenter presenter;
        private SceneSwitcher sceneSwitcher;

        private void Start()
        {
            presenter = new ShopPresenter();
            sceneSwitcher = GetComponent<SceneSwitcher>();

            UpdateTexts();
            presenter.AdvanceLevel();
        }

        private void LateUpdate()
        {
            UpdateTexts();
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

        public void OnContinue()
        {
            sceneSwitcher.SwitchToNext();
        }

        public void OnSave()
        {
            GameView.Instance.SaveGame();
        }
    }
}
using Shop.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.View
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Text coinsText;
        private ShopPresenter presenter;

        private void Start()
        {
            presenter = new ShopPresenter();
            UpdateTexts();
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
    }
}
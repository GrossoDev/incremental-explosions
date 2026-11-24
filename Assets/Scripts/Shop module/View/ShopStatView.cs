using Shop.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.View
{
    public abstract class ShopStatView : MonoBehaviour
    {
        [SerializeField] private Text priceText;
        [SerializeField] private Text levelText;
        [SerializeField] private Button purchaseButton;

        public abstract ShopStatPresenter GetPresenter();

        private void Start()
        {
            UpdateTexts();
            DisableButtonIfNotEnoughCoins();
        }

        public void OnPurchase()
        {
            GetPresenter().Purchase();
            UpdateTexts();
            DisableButtonIfNotEnoughCoins();
        }

        private void UpdateTexts()
        {
            int price = GetPresenter().GetPrice();
            int level = GetPresenter().GetLevel();
            
            priceText.text = $"$ {price}";
            levelText.text = $"Level: {level}";
        }

        private void DisableButtonIfNotEnoughCoins()
        {
            bool canPurchase = GetPresenter().CanPurchase();
            purchaseButton.interactable = canPurchase;
        }
    }
}
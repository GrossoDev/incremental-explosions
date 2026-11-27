using Shared.View;
using Shop.Presentation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.View
{
    public abstract class ShopStatView : MonoBehaviour
    {
        [SerializeField] private Text priceTextComponent;
        [SerializeField] private TMP_Text levelTextComponent;
        [SerializeField] private Button purchaseButton;

        [Header("Strings")]
        [SerializeField] private LocalizedText levelText;

        public abstract ShopStatPresenter GetPresenter();

        private void Start()
        {
            UpdateTexts();
            DisableButtonIfNotEnoughCoins();
        }

        private void Update()
        {
            UpdateTexts();
            DisableButtonIfNotEnoughCoins();
        }

        public void OnPurchase()
        {
            GetPresenter().Purchase();
        }

        private void UpdateTexts()
        {
            int price = GetPresenter().GetPrice();
            int level = GetPresenter().GetLevel();
            
            priceTextComponent.text = $"$ {price}";
            levelTextComponent.text = levelText.GetFormatted(level);
        }

        private void DisableButtonIfNotEnoughCoins()
        {
            bool canPurchase = GetPresenter().CanPurchase();
            purchaseButton.interactable = canPurchase;
        }
    }
}
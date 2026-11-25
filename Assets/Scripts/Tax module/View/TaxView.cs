using System;
using Shared.View;
using Tax.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Tax.View
{

    public class TaxView : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private TaxWarningView warningPanel;
        [SerializeField] private TaxCanPayView canPayPanel;
        [SerializeField] private TaxCannotPayView cannotPayPanel;

        [Header("Battle configuration")]
        [SerializeField] private TaxProgression taxProgression;

        private TaxPresenter presenter;

        private void Start()
        {
            presenter = new TaxPresenter();
            SelectBehaviour();
        }

        private void SelectBehaviour()
        {
            if (DoesCurrentLevelHaveTax())
            {
                bool hasEnoughCoinsToPay = GetTax().tax > GetCurrentCoins();

                canPayPanel.gameObject.SetActive(hasEnoughCoinsToPay);
                cannotPayPanel.gameObject.SetActive(!hasEnoughCoinsToPay);
            }
            else
            {
                warningPanel.gameObject.SetActive(true);
            }
        }

        public TaxPresenter GetPresenter()
        {
            return presenter;
        }

        public bool DoesCurrentLevelHaveTax()
        {
            int currentLevel = GetCurrentLevel();
            return taxProgression.DoesLevelHaveTax(currentLevel);
        }

        public TaxConfig GetTax()
        {
            int currentLevel = GetCurrentLevel();
            return taxProgression.GetTaxConfigForLevel(currentLevel);
        }

        public TaxConfig GetNextTax()
        {
            int currentLevel = GetCurrentLevel();
            return taxProgression.GetNextTax(currentLevel);
        }

        public int GetCurrentLevel()
        {
            return GameView.Instance.GetPresenter().GetCurrentLevel();
        }

        public int GetCurrentCoins()
        {
            return GameView.Instance.GetPresenter().GetTotalCoins();
        }
    }
}
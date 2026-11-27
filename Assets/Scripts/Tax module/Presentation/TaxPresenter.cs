using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Model;
using Tax.View;

namespace Tax.Presentation
{
    public class TaxPresenter
    {
        private TaxView view;
        private List<TaxConfig> taxConfigurations;

        public TaxPresenter(TaxView view)
        {
            this.view = view;
        }

        public void SetTaxProgression(List<TaxConfig> progression)
        {
            taxConfigurations = progression;
        }

        public void SelectBehaviour()
        {
            if (DoesCurrentLevelHaveTax())
            {
                bool hasEnoughCoinsToPay = GetCurrentCoins() >= GetTax().tax;

                if (hasEnoughCoinsToPay)
                {
                    PayTax();
                    view.ShowCanPay();
                }
                else
                {
                    view.ShowCannotPay();
                }
            }
            else
            {
                view.ShowWarning();
            }
        }

        public void PayTax()
        {
            TaxConfig tax = GetTax();
            int currentCoins = Game.Instance.GetTotalCoins();

            Game.Instance.SetTotalCoins(currentCoins - tax.tax);
        }

        public TaxConfig GetTax()
        {
            int currentLevel = GetCurrentLevel();
            return GetTaxConfigForLevel(currentLevel);
        }

        public TaxConfig GetNextTax()
        {
            int currentLevel = GetCurrentLevel();
            var nextTax = taxConfigurations.First(tax => tax.level >= currentLevel);

            return nextTax;
        }

        public bool DoesLevelHaveTax(int level)
        {
            return taxConfigurations.Any(tax => tax.level == level);
        }

        public bool DoesCurrentLevelHaveTax()
        {
            int currentLevel = GetCurrentLevel();
            return DoesLevelHaveTax(currentLevel);
        }

        public TaxConfig GetTaxConfigForLevel(int level)
        {
            foreach (var taxConfig in taxConfigurations)
            {
                if (level == taxConfig.level)
                    return taxConfig;
            }

            throw new Exception($"No tax configuration found for level {level}");
        }

        public int GetCurrentLevel()
        {
            return Game.Instance.GetCurrentLevel();
        }

        public int GetCurrentCoins()
        {
            return Game.Instance.GetTotalCoins();
        }
    }
}
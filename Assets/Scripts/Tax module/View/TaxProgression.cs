using System;
using System.Linq;
using System.Collections.Generic;
using Tax.Presentation;
using UnityEngine;

namespace Tax.View
{
    [CreateAssetMenu(fileName = nameof(TaxProgression), menuName = "Game settings/Tax progression")]
    public class TaxProgression : ScriptableObject
    {
        public List<TaxConfig> taxConfigurations = new List<TaxConfig>();

        public TaxConfig GetNextTax(int currentLevel)
        {
            var nextTax = taxConfigurations.First(tax => tax.level >= currentLevel);

            return nextTax;
        }

        public bool DoesLevelHaveTax(int level)
        {
            return taxConfigurations.Any(tax => tax.level == level);
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
    }
}

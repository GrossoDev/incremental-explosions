using System;

namespace Shared.Model
{
    public abstract class Stat
    {
        protected int level = 1;

        public Stat(int level = 1)
        {
            this.level = level;
        }

        public void Upgrade()
        {
            level++;
        }

        public void Downgrade()
        {
            level = Math.Max(0, level - 1);
        }

        public int GetLevel()
        {
            return level;
        }

        public void Reset()
        {
            level = 1;
        }

        public abstract int GetUpgradeCost();

        protected int CalculateExponentialCost(int baseCost, double growthRate)
        {
            double rawCost = baseCost * Math.Pow(growthRate, Math.Max(0, level - 1));
            return Math.Max(5, (int)(Math.Round(rawCost / 5.0) * 5));
        }
    }
}
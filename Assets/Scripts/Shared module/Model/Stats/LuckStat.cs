using System;

namespace Shared.Model
{
    public class LuckStat : Stat
    {
        private Random random = new Random();

        public LuckStat(int level = 1) : base(level) { }

        public bool RollCrit()
        {
            double randomNumber = random.NextDouble();
            double critThreshold = GetCritChance();

            return randomNumber <= critThreshold;
        }

        public float GetCritChance()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(0.50f, 0.05f + 0.45f * (1f - (float)Math.Exp(-0.08 * (effectiveLevel - 1))));
        }

        public float GetCoinDropMultiplier()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(3.50f, 1.0f + (effectiveLevel - 1) * 0.15f);
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(150, 1.50);
        }

        public override string ToString()
        {
            decimal coinDropMultiplier = decimal.Round((decimal)GetCoinDropMultiplier(), 2);
            int critChance = (int)(GetCritChance() * 100);

            return $"Luck = coin drop mult: {coinDropMultiplier}, crit chance: {critChance}% at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

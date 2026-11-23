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
            return level * 0.05f;
        }

        public float GetCoinDropMultiplier()
        {
            return 1.0f + level * 0.5f;
        }

        public override int GetUpgradeCost()
        {
            return level * level * level;
        }

        public override string ToString()
        {
            decimal coinDropMultiplier = decimal.Round((decimal)GetCoinDropMultiplier(), 2);
            int critChance = (int)(GetCritChance() * 100);

            return $"Luck = coin drop mult: {coinDropMultiplier}, crit chance: {critChance}% at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
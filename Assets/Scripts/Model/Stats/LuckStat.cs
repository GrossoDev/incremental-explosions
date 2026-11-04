using System;

namespace IE.Model
{
    public class LuckStat : Stat
    {
        private Random random = new Random();

        public LuckStat(StatManager statManager) : base(statManager) {}

        public bool RollCrit()
        {
            double randomNumber = random.NextDouble();
            double critThreshold = level * 0.05;

            return randomNumber <= critThreshold;
        }

        public float GetCoinDropMultiplier()
        {
            return 1.0f + level * 0.5f;
        }

        public override int GetUpgradeCost()
        {
            return 10;
        }
    }
}
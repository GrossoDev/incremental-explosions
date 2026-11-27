using System;

namespace Shared.Model
{
    public class SpeedStat : Stat
    {
        public SpeedStat(int level = 1) : base(level) { }

        public int GetTicksBetweenAttacks()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Max(15, (int)Math.Round(15.0 + 85.0 * Math.Pow(0.86, effectiveLevel - 1)));
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(110, 1.46);
        }

        public override string ToString()
        {
            return $"Speed = {GetTicksBetweenAttacks()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

using System;

namespace Shared.Model
{
    public class TimeStat : Stat
    {
        public TimeStat(int level = 1) : base(level) { }

        public int GetRoundTime()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(45, 15 + (int)Math.Round(30.0 * (1.0 - Math.Exp(-0.12 * (effectiveLevel - 1)))));
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(120, 1.48);
        }

        public override string ToString()
        {
            return $"Round time = {GetRoundTime()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

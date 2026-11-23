using System;

namespace Shared.Model
{
    public class SpeedStat : Stat
    {
        public SpeedStat(int level = 1) : base(level) { }

        public int GetTicksBetweenAttacks()
        {
            return Math.Max(100 - level * 10, 1);
        }

        public override int GetUpgradeCost()
        {
            return level * 10;
        }

        public override string ToString()
        {
            return $"Speed = {GetTicksBetweenAttacks()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
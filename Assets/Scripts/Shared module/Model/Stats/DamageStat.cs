using System;

namespace Shared.Model
{
    public class DamageStat : Stat
    {
        public DamageStat(int level = 1) : base(level) { }

        public int GetDamage()
        {
            int effectiveLevel = Math.Max(1, level);
            int offset = effectiveLevel - 1;
            return 6 + offset * 4 + (int)(offset * offset * 0.4);
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(80, 1.42);
        }

        public override string ToString()
        {
            return $"Damage = {GetDamage()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

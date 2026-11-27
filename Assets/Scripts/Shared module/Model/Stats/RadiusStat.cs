using System;

namespace Shared.Model
{
    public class RadiusStat : Stat
    {
        public RadiusStat(int level = 1) : base(level) { }

        public int GetRadius()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(12, 1 + effectiveLevel);
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(140, 1.58);
        }

        public override string ToString()
        {
            return $"Radius = {GetRadius()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

using System;

namespace Shared.Model
{
    public class TorchStat : Stat
    {
        public TorchStat(int level = 1) : base(level) { }

        public float GetLightRange()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(22f, 4.5f + 17.5f * (1f - (float)Math.Exp(-0.12 * (effectiveLevel - 1))));
        }

        public float GetLightIntensity()
        {
            int effectiveLevel = Math.Max(1, level);
            return Math.Min(8f, 4f + 4f * (1f - (float)Math.Exp(-0.10 * (effectiveLevel - 1))));
        }

        public override int GetUpgradeCost()
        {
            return CalculateExponentialCost(90, 1.44);
        }

        public override string ToString()
        {
            return $"Torch = {GetLightRange():0.#} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}

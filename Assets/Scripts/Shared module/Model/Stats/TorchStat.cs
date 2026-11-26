namespace Shared.Model
{
    public class TorchStat : Stat
    {
        public TorchStat(int level = 1) : base(level) { }

        public float GetLightRange()
        {
            return 5.5f + (level - 1) * 1.5f;
        }

        public float GetLightIntensity()
        {
            return 4.5f + (level - 1) * 0.35f;
        }

        public override int GetUpgradeCost()
        {
            return level * 50;
        }

        public override string ToString()
        {
            return $"Torch = {GetLightRange():0.#} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}


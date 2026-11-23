namespace IE.Model
{
    public class RadiusStat : Stat
    {
        public RadiusStat(int level = 1) : base(level) { }

        public int GetRadius()
        {
            return 2 + level;
        }

        public override int GetUpgradeCost()
        {
            return level * level * 10;
        }

        public override string ToString()
        {
            return $"Radius = {GetRadius()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
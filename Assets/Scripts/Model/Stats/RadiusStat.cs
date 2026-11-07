namespace IE.Model
{
    public class RadiusStat : Stat
    {
        public int GetRadius()
        {
            return level;
        }

        public override int GetUpgradeCost()
        {
            return level * 10;
        }
    }
}
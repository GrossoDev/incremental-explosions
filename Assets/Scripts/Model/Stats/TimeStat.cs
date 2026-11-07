namespace IE.Model
{
    public class TimeStat : Stat
    {
        public int GetRoundTime()
        {
            return 15 + level * 5;
        }

        public override int GetUpgradeCost()
        {
            return level * 10;
        }

        public override string ToString()
        {
            return $"Round time = {GetRoundTime()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
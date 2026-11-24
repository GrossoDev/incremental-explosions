namespace Shared.Model
{
    public class TimeStat : Stat
    {
        public TimeStat(int level = 1) : base(level) { }

        public int GetRoundTime()
        {
            return 30 + level * 5;
        }

        public override int GetUpgradeCost()
        {
            return level * 100;
        }

        public override string ToString()
        {
            return $"Round time = {GetRoundTime()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
namespace Shared.Model
{
    public class DamageStat : Stat
    {
        public DamageStat(int level = 1) : base(level) { }

        public int GetDamage()
        {
            return 10 + level;
        }

        public override int GetUpgradeCost()
        {
            return level * 100;
        }

        public override string ToString()
        {
            return $"Damage = {GetDamage()} at level {GetLevel()} (costs ${GetUpgradeCost()} to upgrade)";
        }
    }
}
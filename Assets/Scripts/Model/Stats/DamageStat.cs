namespace IE.Model
{
    public class DamageStat : Stat
{
    public DamageStat(StatManager statManager) : base(statManager) {}

    public int GetDamage()
    {
        return 10 + level;
    }

    public override int GetUpgradeCost()
    {
        return level * 10;
    }
}
}
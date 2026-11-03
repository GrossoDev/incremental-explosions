namespace IE.Model
{
    public class SpeedStat : Stat
{
    public SpeedStat(StatManager statManager) : base(statManager) {}

    public float GetTicksBetweenExplosions()
    {
        return 100 - level * 10;
    }

    public override int GetUpgradeCost()
    {
        return level * 10;
    }
}
}
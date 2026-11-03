namespace IE.Model
{
    public class LuckStat : Stat
{
    public LuckStat(StatManager statManager) : base(statManager) {}

    public float RollCritMultiplier()
    {
        return 1.0f + level * 0.05f;
    }

    public float GetCoinDropMultiplier()
    {
        return 1.0f + level * 0.5f;
    }

    public override int GetUpgradeCost()
    {
        return level * 10;
    }
}
}
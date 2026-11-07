using System;

namespace IE.Model
{
    public class SpeedStat : Stat
    {
        public int GetTicksBetweenAttacks()
        {
            return Math.Max(100 - level * 10, 1);
        }

        public override int GetUpgradeCost()
        {
            return level * 10;
        }
    }
}
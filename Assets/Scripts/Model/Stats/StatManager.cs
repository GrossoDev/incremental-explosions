using System.Collections.Generic;

namespace IE.Model
{
    public class StatManager
    {
        private int totalCoins;
        private List<Stat> stats;
        public static StatManager Instance { get; private set; } = new StatManager();

        private StatManager()
        {
            stats = new List<Stat>()
            {
                new TimeStat(this),
                new SpeedStat(this),
                new RadiusStat(this),
                new LuckStat(this),
                new DamageStat(this),
            };
        }

        public StatType GetStat<StatType>() where StatType : Stat
        {
            Stat stat = stats.Find(stat => stat is StatType);
            
            return stat as StatType;
        }

        public int GetTotalCoins()
        {
            return totalCoins;
        }

        public void AddCoins(int amount)
        {
            totalCoins += amount;
        }

        public void SubtractCoins(int amount)
        {
            totalCoins -= amount;
        }

        public void Reset()
        {
            totalCoins = 0;

            foreach (Stat stat in stats)
            {
                stat.Reset();
            }
        }
    }
}
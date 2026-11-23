using System;
using System.Collections.Generic;
using System.Text;

namespace IE.Model
{
    public class Game
    {
        private int currentLevel;
        private int battleCoins;
        private int totalCoins;
        private List<Stat> stats = new List<Stat>();
        public static Game Instance { get; private set; } = new Game();

        private Game() { }

        public StatType GetStat<StatType>() where StatType : Stat
        {
            Stat stat = stats.Find(stat => stat is StatType);
            
            return stat as StatType;
        }

        public int GetBattleCoins()
        {
            return battleCoins;
        }

        public void SetBattleCoins(int amount)
        {
            battleCoins = amount;
        }

        public int GetTotalCoins()
        {
            return totalCoins;
        }

        public void SetTotalCoins(int amount)
        {
            totalCoins = amount;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
        }

        public void NewGame()
        {
            totalCoins = 0;
            battleCoins = 0;
            currentLevel = 1;
            stats = new List<Stat>()
            {
                new TimeStat(),
                new SpeedStat(),
                new RadiusStat(),
                new LuckStat(),
                new DamageStat(),
            };
        }

        public string Serialize()
        {
            var radiusStatLevel = GetStat<RadiusStat>().GetLevel();
            var speedStatLevel = GetStat<SpeedStat>().GetLevel();
            var damageStatLevel = GetStat<DamageStat>().GetLevel();
            var luckStatLevel = GetStat<LuckStat>().GetLevel();
            var timeStatLevel = GetStat<TimeStat>().GetLevel();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(currentLevel.ToString());
            sb.AppendLine(totalCoins.ToString());
            sb.AppendLine(radiusStatLevel.ToString());
            sb.AppendLine(speedStatLevel.ToString());
            sb.AppendLine(damageStatLevel.ToString());
            sb.AppendLine(luckStatLevel.ToString());
            sb.AppendLine(timeStatLevel.ToString());

            return sb.ToString();
        }

        public void Deserialize(string serialized)
        {
            string[] lines = serialized.Split("\n");

            currentLevel = int.Parse(lines[0]);
            totalCoins = int.Parse(lines[1]);

            var radiusStatLevel = int.Parse(lines[2]);
            var speedStatLevel = int.Parse(lines[3]);
            var damageStatLevel = int.Parse(lines[4]);
            var luckStatLevel = int.Parse(lines[5]);
            var timeStatLevel = int.Parse(lines[6]);

            stats = new List<Stat>()
            {
                new RadiusStat(radiusStatLevel),
                new SpeedStat(speedStatLevel),
                new DamageStat(damageStatLevel),
                new LuckStat(luckStatLevel),
                new TimeStat(timeStatLevel),
            };
        }
    }
}
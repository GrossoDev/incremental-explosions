using System;
using System.Collections.Generic;

namespace IE.Model
{
    public class Game
    {
        private int currentLevel;
        private int totalCoins;
        private List<Stat> stats = new List<Stat>();
        public static Game Instance { get; private set; } = new Game();

        private Game() { }

        public void AddCoins(int amount)
        {
            totalCoins += amount;
        }

        public void SubtractCoins(int amount)
        {
            totalCoins -= amount;
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
            currentLevel = 0;
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
            return "";
        }

        public void Deserialize(string serializedGame)
        {
            
        }
    }
}
using System;
using System.Collections.Generic;

namespace IE.Model
{
    public class Game
    {
        private const int Width = 40;
        private const int Height = 40;
        private Arena arena;
        private Player player;
        private int totalCoins;
        private List<Stat> stats;
        public static Game Instance { get; private set; } = new Game();

        public void Initialize()
        {
            CreateStats();
            CreateArena();
            CreatePlayer();
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
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

        public Arena GetArena()
        {
            return arena;
        }

        public Player GetPlayer()
        {
            return player;
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

        private void CreatePlayer()
        {
            Cell playerCell = arena.GetRandomFreeCell();
            player = new Player(playerCell, GridDirection.Up);
        }

        private void CreateStats()
        {
            stats = new List<Stat>()
            {
                new TimeStat(),
                new SpeedStat(),
                new RadiusStat(),
                new LuckStat(),
                new DamageStat(),
            };
        }

        private void CreateArena()
        {
            arena = new Arena();
            arena.Initialize(Width, Height);
        }
    }
}
using System;
using Shared.Model;

namespace Battle.Model
{
    public class Battle
    {
        private const int Width = 40;
        private const int Height = 40;
        private int roundTime = 40;
        private int remainingTime = 40;
        private Arena arena;
        private Player player;

        public void Initialize()
        {
            Game.Instance.SetBattleCoins(0);

            var timeStat = Game.Instance.GetStat<TimeStat>();
            roundTime = timeStat.GetRoundTime();
            remainingTime = roundTime;
            
            CreateArena();
            CreatePlayer();
        }
        
        public void SecondElapsed()
        {
            remainingTime = Math.Max(remainingTime - 1, 0);
        }

        public Arena GetArena()
        {
            return arena;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public bool IsRoundOver()
        {
            return remainingTime <= 0;
        }

        public int GetRoundTime()
        {
            return roundTime;
        }

        public int GetRemainingTime()
        {
            return remainingTime;
        }

        private void CreatePlayer()
        {
            Cell playerCell = arena.GetRandomFreeCell();
            player = new Player(playerCell, GridDirection.Up);
        }

        private void CreateArena()
        {
            arena = new Arena();
            arena.Initialize(Width, Height);
        }
    }
}
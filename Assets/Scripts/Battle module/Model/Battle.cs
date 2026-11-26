using System;
using Battle.Presentation;
using Shared.Model;

namespace Battle.Model
{
    public class Battle
    {
        private int width = 40;
        private int height = 40;
        private int roundTime = 40;
        private int remainingTime = 40;
        private Arena arena;
        private Player player;
        private BattleConfig battleConfig;

        public void Initialize(BattleConfig battleConfig)
        {
            Game.Instance.SetBattleCoins(0);

            this.width = battleConfig.width;
            this.height = battleConfig.height;
            this.battleConfig = battleConfig;
            
            var timeStat = Game.Instance.GetStat<TimeStat>();
            roundTime = timeStat.GetRoundTime();
            remainingTime = roundTime;
            
            CreateArena();
            CreatePlayer();
            SpawnEnemies();
        }
        
        public void SecondElapsed()
        {
            remainingTime = Math.Max(remainingTime - 1, 0);

            bool allEnemiesKilled = !arena.AreThereAnyEntitiesOfType<Enemy>();

            if (allEnemiesKilled)
                SpawnEnemies();
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
            arena.Initialize(width, height);
        }

        public void SpawnEnemies()
        {
            foreach (var spawnConfig in battleConfig.enemiesToSpawn)
            {
                Type enemyType = spawnConfig.enemyType.GetSerializedType();
                int amount = RandomUtils.GetRandomInt(spawnConfig.minimumAmount, spawnConfig.maximumAmount);

                SpawnEntities(enemyType, amount);
            }
        }

        public void SpawnEntities(Type entityType, int amount)
        {
            if (!entityType.IsSubclassOf(typeof(Entity)))
                throw new ArgumentException($"Tried to spawn {entityType.Name}, which is not an entity.");

            for (int i = 0; i < amount; i++)
            {
                Cell cell = arena.GetRandomFreeCell();

                Activator.CreateInstance(entityType, cell, GridDirection.Down);
            }
        }
    }
}
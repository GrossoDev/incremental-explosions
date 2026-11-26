using System;
using Shared.Model;
using Battle.Model;
using Battle.View;
using System.Collections.Generic;
using System.Linq;

namespace Battle.Presentation
{
    public class BattlePresenter
    {
        private Model.Battle model;
        private BattleView view;
        private List<BattleConfig> battleProgression;

        public BattlePresenter(BattleView battleView)
        {
            view = battleView;
            model = new Model.Battle();
        }

        public void SecondElapsed()
        {
            model.SecondElapsed();

            if (model.IsRoundOver())
            {
                view.BattleEnded();
            }
        }

        public int GetCollectedCoins()
        {
            return Game.Instance.GetBattleCoins();
        }

        public int GetRoundTime()
        {
            return model.GetRoundTime();
        }

        public int GetRemainingTime()
        {
            return model.GetRemainingTime();
        }

        public bool IsRoundOver()
        {
           return model.IsRoundOver();
        }

        public void SetBattleProgression(List<BattleConfig> battleConfigurations)
        {
            this.battleProgression = battleConfigurations;
        }

        public BattleConfig GetBattleConfigForLevel(int level)
        {
            foreach (var battleConfig in battleProgression.Reverse<BattleConfig>())
            {
                if (level >= battleConfig.startingLevel)
                {
                    return battleConfig;
                }
            }

            return battleProgression.First();
        }

        public int GetCurrentLevel()
        {
            return Game.Instance.GetCurrentLevel();
        }

        public void CreateBattle()
        {
            int currentLevel = GetCurrentLevel();
            BattleConfig battleConfig = GetBattleConfigForLevel(currentLevel);

            model.Initialize(battleConfig.width, battleConfig.height);

            foreach (var spawnConfig in battleConfig.enemiesToSpawn)
            {
                Type enemyType = spawnConfig.enemyType.GetSerializedType();
                int amount = RandomUtils.GetRandomInt(spawnConfig.minimumAmount, spawnConfig.maximumAmount);

                SpawnEntities(enemyType, amount);
            }
        }

        public void SpawnEntities(Type entityType, int amount)
        {
            Arena arena = model.GetArena();

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
using System;
using System.Collections.Generic;
using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class BattlePresenter
    {
        private Battle model;

        public BattlePresenter()
        {
            model = new Battle();
            model.Initialize();
        }

        public Arena GetArena()
        {
            return model.GetArena();
        }

        public void CreateBattle(BattleConfig battleConfig)
        {
            foreach (var spawnConfig in battleConfig.enemiesToSpawn)
            {
                Type enemyType = spawnConfig.enemyType.GetSerializedType();
                int amount = RandomUtils.GetRandomInt(spawnConfig.minimumAmount, spawnConfig.maximumAmount);

                SpawnEnemies(enemyType, amount);
            }
        }

        private void SpawnEnemies(Type enemyType, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Cell cell = GetArena().GetRandomFreeCell();

                Activator.CreateInstance(enemyType, cell, GridDirection.Down);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Battle.Model;
using UnityEngine;

namespace Battle.Presentation
{
    [Serializable]
    public class BattleConfig
    {
        public int startingLevel = 0;
        public int width = 40;
        public int height = 40;
        public List<EnemySpawnConfig> enemiesToSpawn = new List<EnemySpawnConfig>();

        [Serializable]
        public class EnemySpawnConfig
        {
            [HideInInspector] public string displayName;
            [SubclassOf(typeof(Enemy))] public SerializableType enemyType;
            public int minimumAmount;
            public int maximumAmount;
        }
    }
}

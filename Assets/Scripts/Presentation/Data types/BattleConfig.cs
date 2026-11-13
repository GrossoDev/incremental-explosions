using System;
using System.Collections.Generic;
using IE.Model;
using UnityEngine;

namespace IE.Presentation
{
    [Serializable]
    public class BattleConfig
    {
        public int startingLevel = 0;
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

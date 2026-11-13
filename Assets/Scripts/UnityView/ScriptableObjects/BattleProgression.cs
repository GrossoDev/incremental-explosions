using System;
using System.Collections.Generic;
using IE.Model;
using UnityEngine;

namespace IE.UnityView
{
    [CreateAssetMenu(fileName = nameof(BattleProgression), menuName = "Game settings/Battle progression")]
    public class BattleProgression : ScriptableObject
    {
        public List<BattleConfig> battleConfigurations = new List<BattleConfig>();

        private void OnValidate()
        {
            if (battleConfigurations == null) return;

            foreach (var config in battleConfigurations)
            {
                foreach (var spawnConfig in config.enemiesToSpawn)
                {
                    if (string.IsNullOrWhiteSpace(spawnConfig.enemyType.assemblyQualifiedName))
                    {
                        spawnConfig.displayName = "Not set";
                    }
                    else
                    {
                        string enemyTypeName = spawnConfig.enemyType.GetSerializedType().Name;
                        int min = spawnConfig.minimumAmount;
                        int max = spawnConfig.maximumAmount;

                        spawnConfig.displayName = $"{enemyTypeName}: between {min} and {max}";
                    }
                }
            }
        }
    }

    [Serializable]
    public class BattleConfig
    {
        public int fromLevel = 0;
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

using System.Linq;
using System.Collections.Generic;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    [CreateAssetMenu(fileName = nameof(BattleProgression), menuName = "Game settings/Battle progression")]
    public class BattleProgression : ScriptableObject
    {
        public List<BattleConfig> battleConfigurations = new List<BattleConfig>();

        public BattleConfig GetBattleConfigForLevel(int level)
        {
            foreach (var battleConfig in battleConfigurations.Reverse<BattleConfig>())
            {
                if (level >= battleConfig.startingLevel)
                {
                    return battleConfig;
                }
            }

            return battleConfigurations.First();
        }

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
}

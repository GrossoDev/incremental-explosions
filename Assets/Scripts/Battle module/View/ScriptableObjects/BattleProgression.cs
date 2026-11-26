using System.Linq;
using System.Collections.Generic;
using Battle.Presentation;
using UnityEngine;

namespace Battle.View
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
}

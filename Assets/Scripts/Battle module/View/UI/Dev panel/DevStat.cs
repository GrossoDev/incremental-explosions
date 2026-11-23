using Shared.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.View
{
    public abstract class DevStat<EntityType> : MonoBehaviour where EntityType : Stat
    {
        [SerializeField] private Text textElement;

        private void Update()
        {
            var text = Game.Instance.GetStat<EntityType>().ToString();
            textElement.text = text;
        }

        public void Upgrade()
        {
            Game.Instance.GetStat<EntityType>().Upgrade();
        }

        public void Downgrade()
        {
            Game.Instance.GetStat<EntityType>().Downgrade();
        }
    }
}
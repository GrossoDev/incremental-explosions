using IE.Model;
using UnityEngine;
using UnityEngine.UI;

namespace IE.UnityView
{
    public abstract class DevEntitySpawner<EntityType> : MonoBehaviour where EntityType : Entity
    {
        [SerializeField] private TMPro.TMP_InputField spawnAmountField;
        [SerializeField] private BattleView battleView;

        public void Spawn()
        {
            var text = spawnAmountField.text;

            if (!int.TryParse(text, out int amount))
                return;
            
            for (int i = 0; i < amount; i++)
            {
                SpawnEntity(GetRandomCell());
            }
        }

        private Cell GetRandomCell()
        {
            return battleView.GetArena().GetRandomFreeCell();
        }

        protected abstract void SpawnEntity(Cell cell);
    }
}
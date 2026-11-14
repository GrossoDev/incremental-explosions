using TMPro;
using IE.Model;
using UnityEngine;

namespace IE.UnityView
{
    public class DevEntitySpawner : MonoBehaviour
    {
        [SerializeField] [SubclassOf(typeof(Entity))] private SerializableType entity;
        [SerializeField] private TMP_InputField spawnAmountField;
        [SerializeField] private BattleView battleView;

        public void Spawn()
        {
            var text = spawnAmountField.text;
            var entityType = entity.GetSerializedType();
            var battlePresenter = battleView.GetPresenter();

            if (!int.TryParse(text, out int amount))
                return;
            
            battlePresenter.SpawnEntities(entityType, amount);
        }
    }
}
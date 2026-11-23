using Shared.View;
using UnityEngine;

namespace Battle.View
{
    public class DevPanelSaveGame : MonoBehaviour
    {
        public void OnSaveGame()
        {
            GameView.Instance.SaveGame();

            var serialized = GameView.Instance.GetPresenter().Serialize();
            Debug.Log($"Serialized game:\n{serialized}");
        }
    }
}
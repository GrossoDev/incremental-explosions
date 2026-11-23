using UnityEngine;

namespace IE.UnityView
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
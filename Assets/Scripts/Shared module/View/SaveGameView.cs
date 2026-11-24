using UnityEngine;

namespace Shared.View
{
    public class SaveGameView : MonoBehaviour
    {
        public void OnSaveGame()
        {
            GameView.Instance.SaveGame();

            var serialized = GameView.Instance.GetPresenter().Serialize();
            Debug.Log($"Serialized game:\n{serialized}");
        }
    }
}
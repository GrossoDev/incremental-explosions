using UnityEngine;

namespace IE.UnityView
{
    public class MockGameView : MonoBehaviour
    {
        private void OnEnable()
        {
            if (GameView.Instance is null)
            {
                this.gameObject.AddComponent<GameView>();
                GameView.Instance.StartNewGame();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
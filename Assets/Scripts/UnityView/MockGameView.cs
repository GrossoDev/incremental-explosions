using UnityEngine;

namespace IE.UnityView
{
    public class MockGameView : GameView
    {
        [SerializeField] [Range(1,50)] private int level = 1;

        private void OnEnable()
        {
            StartNewGame();
            gamePresenter.SetCurrentLevel(level);
        }
    }
}
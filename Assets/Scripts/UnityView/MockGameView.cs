using UnityEngine;

namespace IE.UnityView
{
    public class MockGameView : GameView
    {
        [SerializeField] [Range(1,50)] private int fakeLevel = 1;

        private void OnEnable()
        {
            StartNewGame();
            gamePresenter.SetCurrentLevel(fakeLevel);
        }
    }
}
using UnityEngine;
using IE.Presentation;

namespace IE.UnityView
{
    [RequireComponent(typeof(GameView))]
    public abstract class GameInputCommand : InputCommand
    {
        protected GamePresenter gamePresenter;

        protected void Awake()
        {
            GameView gameView = GetComponent<GameView>();
            gamePresenter = gameView.GetPresenter();
        }
    }
}
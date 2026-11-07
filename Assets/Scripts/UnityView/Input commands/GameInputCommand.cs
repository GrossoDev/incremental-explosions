using UnityEngine;

namespace IE.UnityView
{
    [RequireComponent(typeof(GameView))]
    public abstract class GameInputCommand : InputCommand
    {
        protected GameView gameView;

        protected void Start()
        {
            gameView = GetComponent<GameView>();
        }
    }
}
using UnityEngine;
using Battle.Presentation;

namespace Battle.View
{
    [RequireComponent(typeof(PlayerView))]
    public abstract class PlayerInputCommand : InputCommand
    {
        protected PlayerPresenter playerPresenter;

        protected void Start()
        {
            PlayerView playerView = GetComponent<PlayerView>();
            playerPresenter = playerView.GetPresenter();
        }
    }
}
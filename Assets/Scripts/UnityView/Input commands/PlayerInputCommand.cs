using UnityEngine;
using IE.Presentation;

namespace IE.UnityView
{
    [RequireComponent(typeof(PlayerView))]
    public abstract class PlayerInputCommand : InputCommand
    {
        protected PlayerPresenter playerPresenter;

        protected void Awake()
        {
            PlayerView playerView = GetComponent<PlayerView>();
            playerPresenter = playerView.GetPresenter();
        }
    }
}
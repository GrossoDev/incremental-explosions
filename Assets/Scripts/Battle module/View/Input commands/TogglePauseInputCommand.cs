using Battle.Presentation;
using UnityEngine;

namespace Battle.View
{
    public class TogglePauseInputCommand : InputCommand
    {
        [SerializeField] private PauseView pauseView;
        private PausePresenter pausePresenter;

        private void Start()
        {
            pausePresenter = pauseView.GetPresenter();
        }

        protected override void OnKeyDown()
        {
            pausePresenter.TogglePause();
        }
    }
}

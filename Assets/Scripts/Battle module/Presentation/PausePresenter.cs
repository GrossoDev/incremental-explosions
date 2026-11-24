using System;
using Battle.View;

namespace Battle.Presentation
{
    public class PausePresenter
    {
        private PauseView view;
        private bool isPaused;

        public PausePresenter(PauseView pauseView)
        {
            view = pauseView ?? throw new ArgumentNullException(nameof(pauseView));
        }

        public bool IsPaused()
        {
            return isPaused;
        }

        public void TogglePause()
        {
            SetPaused(!isPaused);
        }

        public void Pause()
        {
            if (isPaused)
                return;

            SetPaused(true);
        }

        public void Resume()
        {
            SetPaused(false);
        }

        public void SaveAndExit()
        {
            Resume();
            view.SaveAndExitToMenu();
        }

        private void SetPaused(bool paused)
        {
            isPaused = paused;
            view.SetPaused(isPaused);
        }
    }
}


using Battle.Presentation;
using UnityEngine;

namespace Battle.View
{
    public class PauseView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private BattleView battleView;

        private PausePresenter presenter;

        private void Awake()
        {
            presenter = new PausePresenter(this);
        }

        private void Start()
        {
            presenter.Resume();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        public PausePresenter GetPresenter()
        {
            return presenter;
        }

        public void TogglePause()
        {
            presenter.TogglePause();
        }

        public void Pause()
        {
            presenter.Pause();
        }

        public void Resume()
        {
            presenter.Resume();
        }

        public void OnSaveAndExit()
        {
            presenter.SaveAndExit();
        }

        public void SetPaused(bool isPaused)
        {
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }

        public void SaveAndExitToMenu()
        {
            battleView.SaveAndExitToMenu();
        }
    }
}
using Battle.View;
using UnityEngine;

public class PauseView : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private BattleView battleView;

    private bool _isPaused;

    private void Start()
    {
        Resume();
    }

    public void TogglePause()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        _isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnSaveAndExit()
    {
        Time.timeScale = 1f; // si no restauras el tiempo, el juego se quedará pausado al volver al menú

        battleView.SaveAndExitToMenu();
    }
}
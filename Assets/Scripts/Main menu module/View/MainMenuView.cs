using System;
using Shared.View;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button loadGameButton;

        private GameView gameView;
        private SceneSwitcher sceneSwitcher;

        public void Start()
        {
            gameView = GameView.Instance;
            sceneSwitcher = GetComponent<SceneSwitcher>();

            DisableLoadButtonIfNoSaveFileExists();
        }

        private void DisableLoadButtonIfNoSaveFileExists()
        {
            bool saveFileExists = gameView.DoesSavedGameExist();

            loadGameButton.interactable = saveFileExists;
        }

        public void OnStartNewGame()
        {
            gameView.StartNewGame();
            sceneSwitcher.SwitchToNext();
        }

        public void OnLoadGame()
        {
            gameView.LoadGame();
            sceneSwitcher.SwitchToNext();
        }
    }
}
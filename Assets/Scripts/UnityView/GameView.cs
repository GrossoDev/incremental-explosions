using System;
using System.Collections;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class GameView : MonoBehaviour
    {
        private const string PlayerPrefsSaveKey = "savefile";
        private GamePresenter gamePresenter;

        public static GameView Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            gamePresenter = new GamePresenter();
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void StartNewGame()
        {
            gamePresenter.StartNewGame();
        }

        public void LoadGame()
        {
            string serializedGame = PlayerPrefs.GetString(PlayerPrefsSaveKey);
            gamePresenter.Deserialize(serializedGame);
        }

        public void SaveGame()
        {
            string serializedGame = gamePresenter.Serialize();
            PlayerPrefs.SetString(PlayerPrefsSaveKey, serializedGame);
        }

        public bool DoesSavedGameExist()
        {
            return PlayerPrefs.HasKey(PlayerPrefsSaveKey);
        }

        public GamePresenter GetPresenter()
        {
            return gamePresenter;
        }
    }
}
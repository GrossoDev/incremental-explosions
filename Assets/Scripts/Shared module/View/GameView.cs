using Shared.Presentation;
using UnityEngine;

namespace Shared.View
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] protected Localization localization;

        private const string PlayerPrefsSaveKey = "savefile";
        protected GamePresenter gamePresenter;

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
            if (Instance == this)
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
            PlayerPrefs.Save();
        }

        public void DeleteSavedGame()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsSaveKey);
            PlayerPrefs.Save();
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
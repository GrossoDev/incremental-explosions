using System.Collections;
using TMPro;
using UnityEngine;
using Shared.View;
using Battle.Presentation;

namespace Battle.View
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class BattleView : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI levelCounterText;
        [SerializeField] private TextMeshProUGUI timeRemainingText;

        [Header("Battle configuration")]
        [SerializeField] private BattleProgression battleProgression;

        private GameView gameView;
        private SceneSwitcher sceneSwitcher;
        private BattlePresenter battlePresenter;

        public void Start()
        {
            gameView = GameView.Instance;
            sceneSwitcher = GetComponent<SceneSwitcher>();
            battlePresenter = new BattlePresenter(this);

            CreateBattle();
            StartCoroutine(CountdownSeconds());
        }

        public void FixedUpdate()
        {
            int coinsCollected = battlePresenter.GetCollectedCoins();
            coinsText.text = $"Coins: {coinsCollected}";
        }

        public IEnumerator CountdownSeconds()
        {
            while (!battlePresenter.IsRoundOver())
            {
                UpdateTimeCounter();
                yield return new WaitForSeconds(1);
                battlePresenter.SecondElapsed();
            }
        }

        public BattlePresenter GetPresenter()
        {
            return battlePresenter;
        }

        public void BattleEnded()
        {
            Time.timeScale = 1f;
            sceneSwitcher.SwitchToNext();
        }

        private void CreateBattle()
        {
            var currentLevel = gameView.GetPresenter().GetCurrentLevel();
            var battleConfig = battleProgression.GetBattleConfigForLevel(currentLevel);

            battlePresenter.CreateBattle(battleConfig);
            levelCounterText.text = $"Level {currentLevel}";
        }

        private void UpdateTimeCounter()
        {
            string timeleftString = $"{battlePresenter.GetRemainingTime()} / {battlePresenter.GetRoundTime()}";
            timeRemainingText.text = timeleftString;
        }

        public void SaveAndExitToMenu()
        {
            Time.timeScale = 1f;
            gameView.SaveGame();
            sceneSwitcher.SwitchToNext();
        }
    }
}
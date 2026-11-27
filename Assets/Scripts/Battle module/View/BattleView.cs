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
        [SerializeField] private TMP_Text coinsTextComponent;
        [SerializeField] private TMP_Text levelTextComponent;
        [SerializeField] private TMP_Text timeTextComponent;

        [Header("Strings")]
        [SerializeField] private LocalizedText coinsText;
        [SerializeField] private LocalizedText levelText;
        [SerializeField] private LocalizedText timeText;

        [Header("Battle configuration")]
        [SerializeField] private BattleProgression battleProgression;

        [Header("Navigation")]
        [SerializeField] private SceneField mainMenuScene;

        private GameView gameView;
        private SceneSwitcher sceneSwitcher;
        private BattlePresenter battlePresenter;

        public void Start()
        {
            gameView = GameView.Instance;
            sceneSwitcher = GetComponent<SceneSwitcher>();
            battlePresenter = new BattlePresenter(this);
            battlePresenter.SetBattleProgression(battleProgression.battleConfigurations);

            UpdateLevelText();
            battlePresenter.CreateBattle();
            StartCoroutine(CountdownSeconds());
        }

        public void FixedUpdate()
        {
            int coinsCollected = battlePresenter.GetCollectedCoins();
            coinsTextComponent.text = coinsText.GetFormatted(coinsCollected);
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

        private void UpdateLevelText()
        {
            int currentLevel = battlePresenter.GetCurrentLevel();
            levelTextComponent.text = levelText.GetFormatted(currentLevel);
        }

        private void UpdateTimeCounter()
        {
            string timeleftString = timeText.GetFormatted(battlePresenter.GetRemainingTime(), battlePresenter.GetRoundTime());
            timeTextComponent.text = timeleftString;
        }

        public void ExitToMenu()
        {
            Time.timeScale = 1f;
            sceneSwitcher.SwitchTo(mainMenuScene);
        }
    }
}
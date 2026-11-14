using System.Collections;
using System.Linq;
using IE.Model;
using IE.Presentation;
using UnityEditor;
using UnityEngine;

namespace IE.UnityView
{
    [RequireComponent(typeof(SceneSwitcher))]
    public class BattleView : MonoBehaviour
    {
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

        public IEnumerator CountdownSeconds()
        {
            while (!battlePresenter.IsRoundOver())
            {
                yield return new WaitForSeconds(1);
                battlePresenter.SecondElapsed();
                UpdateTimeCounter();
            }
        }

        public BattlePresenter GetPresenter()
        {
            return battlePresenter;
        }

        public void BattleEnded()
        {
            sceneSwitcher.SwitchToNext();
        }

        private void CreateBattle()
        {
            var currentLevel = gameView.GetPresenter().GetCurrentLevel();
            var battleConfig = battleProgression.GetBattleConfigForLevel(currentLevel);

            battlePresenter.CreateBattle(battleConfig);
        }

        private void UpdateTimeCounter()
        {
            Debug.Log($"{battlePresenter.GetRemainingTime()} of {battlePresenter.GetRoundTime()}");
        }
    }
}
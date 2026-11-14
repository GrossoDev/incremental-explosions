using System;
using System.Linq;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private BattleProgression battleProgression;

        private GameView gameView;
        private BattlePresenter battlePresenter;

        public void Start()
        {
            gameView = GameView.Instance;
            battlePresenter = new BattlePresenter();
            
            var currentLevel = gameView.GetPresenter().GetCurrentLevel();
            var battleConfig = battleProgression.GetBattleConfigForLevel(currentLevel);

            battlePresenter.CreateBattle(battleConfig);
        }

        

        public Arena GetArena()
        {
            // Está acá porque lo necesita el DevPanel
            // Por ahí convendría agregar un DevPanelPresenter para arreglar esto
            Debug.LogWarning("che hay que borrar esto, la vista no deberia tocar el modelo");
            return battlePresenter.GetArena();
        }

        public BattlePresenter GetPresenter()
        {
            return battlePresenter;
        }
    }
}
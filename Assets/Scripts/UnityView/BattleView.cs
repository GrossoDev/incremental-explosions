using System;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private BattleProgression battleProgression;

        private BattlePresenter battlePresenter;
        private GameView gameView;

        public void Start()
        {
            gameView = GameView.Instance;
            battlePresenter = new BattlePresenter();
        }

        public Arena GetArena()
        {
            // Está acá porque lo necesita el DevPanel
            // Por ahí convendría agregar un DevPanelPresenter para arreglar esto
            Debug.LogWarning("che hay que borrar esto, la vista no deberia tocar el modelo");
            return battlePresenter.GetArena();
        }
    }
}
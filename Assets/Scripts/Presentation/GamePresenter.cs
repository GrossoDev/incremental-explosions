using System;
using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class GamePresenter
    {
        private Game model;

        public GamePresenter()
        {
            model = Game.Instance;
        }

        public int GetTotalCoins()
        {
            return model.GetTotalCoins();
        }

        public int GetCurrentLevel()
        {
            return model.GetCurrentLevel();
        }

        public void SetCurrentLevel(int level)
        {
            model.SetCurrentLevel(level);
        }

        public void StartNewGame()
        {
            model.NewGame();
        }

        public string Serialize()
        {
            return model.Serialize();
        }

        public void Deserialize(string serializedGame)
        {
            model.Deserialize(serializedGame);
        }
    }
}
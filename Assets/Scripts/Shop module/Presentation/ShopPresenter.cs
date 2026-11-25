using System;
using Shared.Model;

namespace Shop.Presentation
{
    public class ShopPresenter
    {
        public int GetCoins()
        {
            return Game.Instance.GetTotalCoins();
        }

        public void AdvanceLevel()
        {
            int currentLevel = Game.Instance.GetCurrentLevel();

            Game.Instance.SetCurrentLevel(currentLevel + 1);
        }
    }
}
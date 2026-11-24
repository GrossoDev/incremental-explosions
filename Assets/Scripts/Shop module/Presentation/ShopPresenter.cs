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
    }
}
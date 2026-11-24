using System;
using Shared.Model;

namespace Shop.Presentation
{
    public abstract class ShopStatPresenter
    {
        protected Stat stat;

        protected ShopStatPresenter()
        {
            Initialize();
        }

        public abstract void Initialize();
        
        public bool CanPurchase()
        {
            int coins = Game.Instance.GetTotalCoins();
            int cost = stat.GetUpgradeCost();

            return coins >= cost;
        }

        public int GetPrice()
        {
            return stat.GetUpgradeCost();
        }

        public int GetLevel()
        {
            return stat.GetLevel();
        }

        public void Purchase()
        {
            if (!CanPurchase())
                throw new Exception($"Tried to upgrade {stat.GetType().Name} but there's not enough coins.");

            int coins = Game.Instance.GetTotalCoins();
            int cost = stat.GetUpgradeCost();

            stat.Upgrade();
            Game.Instance.SetTotalCoins(coins - cost);
        }
    }

    public class DamageShopStatPresenter : ShopStatPresenter
    {
        public override void Initialize()
        {
            stat = Game.Instance.GetStat<DamageStat>();
        }
    }

    public class RadiusShopStatPresenter : ShopStatPresenter
    {
        public override void Initialize()
        {
            stat = Game.Instance.GetStat<RadiusStat>();
        }
    }

    public class LuckShopStatPresenter : ShopStatPresenter
    {
        public override void Initialize()
        {
            stat = Game.Instance.GetStat<LuckStat>();
        }
    }

    public class TimeShopStatPresenter : ShopStatPresenter
    {
        public override void Initialize()
        {
            stat = Game.Instance.GetStat<TimeStat>();
        }
    }

    public class SpeedShopStatPresenter : ShopStatPresenter
    {
        public override void Initialize()
        {
            stat = Game.Instance.GetStat<SpeedStat>();
        }
    }
}
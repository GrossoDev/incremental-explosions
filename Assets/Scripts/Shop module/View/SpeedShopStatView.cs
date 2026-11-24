using Shop.Presentation;

namespace Shop.View
{
    public class SpeedShopStatView : ShopStatView
    {
        private SpeedShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new SpeedShopStatPresenter();

            return presenter;
        }
    }
}
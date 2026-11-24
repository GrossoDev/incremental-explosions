using Shop.Presentation;

namespace Shop.View
{
    public class RadiusShopStatView : ShopStatView
    {
        private RadiusShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new RadiusShopStatPresenter();

            return presenter;
        }
    }
}
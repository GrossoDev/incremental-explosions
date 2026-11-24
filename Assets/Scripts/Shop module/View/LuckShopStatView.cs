using Shop.Presentation;

namespace Shop.View
{
    public class LuckShopStatView : ShopStatView
    {
        private LuckShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new LuckShopStatPresenter();

            return presenter;
        }
    }
}
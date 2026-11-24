using Shop.Presentation;

namespace Shop.View
{
    public class DamageShopStatView : ShopStatView
    {
        private DamageShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new DamageShopStatPresenter();

            return presenter;
        }
    }
}
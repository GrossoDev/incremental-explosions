using Shop.Presentation;

namespace Shop.View
{
    public class TorchShopStatView : ShopStatView
    {
        private TorchShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new TorchShopStatPresenter();

            return presenter;
        }
    }
}


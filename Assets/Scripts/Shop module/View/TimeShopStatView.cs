using Shop.Presentation;

namespace Shop.View
{
    public class TimeShopStatView : ShopStatView
    {
        private TimeShopStatPresenter presenter;

        public override ShopStatPresenter GetPresenter()
        {
            if (presenter is null)
                presenter = new TimeShopStatPresenter();

            return presenter;
        }
    }
}
using Battle.Presentation;

namespace Battle.View
{
    public class PlayerView : EntityView
    {
        public PlayerPresenter GetPresenter()
        {
            return (PlayerPresenter)presenter;
        }
    }
}
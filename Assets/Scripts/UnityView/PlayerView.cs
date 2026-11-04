using IE.Presentation;

namespace IE.UnityView
{
    public class PlayerView : EntityView
    {
        public PlayerPresenter GetPresenter()
        {
            return (PlayerPresenter)presenter;
        }
    }
}
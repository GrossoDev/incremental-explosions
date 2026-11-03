namespace IE.UnityView
{
    public class GoLeftInputCommand : PlayerInputCommand
    {
        protected override void OnKeyDown()
        {
            playerPresenter.GoLeft();
        }
    }
}
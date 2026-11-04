namespace IE.UnityView
{
    public class GoLeftInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoLeft();
        }
    }
}
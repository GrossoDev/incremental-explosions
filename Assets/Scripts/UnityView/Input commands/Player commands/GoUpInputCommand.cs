namespace IE.UnityView
{
    public class GoUpInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoUp();
        }
    }
}
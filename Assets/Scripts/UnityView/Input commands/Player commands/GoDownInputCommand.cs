namespace IE.UnityView
{
    public class GoDownInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoDown();
        }
    }
}
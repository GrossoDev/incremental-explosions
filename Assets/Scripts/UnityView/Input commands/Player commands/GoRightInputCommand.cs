namespace IE.UnityView
{
    public class GoRightInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoRight();
        }
    }
}
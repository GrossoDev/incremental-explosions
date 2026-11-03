namespace IE.UnityView
{
    public class GoRightInputCommand : PlayerInputCommand
    {
        protected override void OnKeyDown()
        {
            playerPresenter.GoRight();
        }
    }
}
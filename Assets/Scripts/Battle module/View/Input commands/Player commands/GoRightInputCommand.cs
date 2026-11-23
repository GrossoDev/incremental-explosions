namespace Battle.View
{
    public class GoRightInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoRight();
        }
    }
}
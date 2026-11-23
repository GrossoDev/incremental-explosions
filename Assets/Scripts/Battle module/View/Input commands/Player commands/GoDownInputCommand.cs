namespace Battle.View
{
    public class GoDownInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoDown();
        }
    }
}
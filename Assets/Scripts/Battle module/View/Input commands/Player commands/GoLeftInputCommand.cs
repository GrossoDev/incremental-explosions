namespace Battle.View
{
    public class GoLeftInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoLeft();
        }
    }
}
namespace Battle.View
{
    public class GoUpInputCommand : PlayerInputCommand
    {
        protected override void OnKeyHeld()
        {
            playerPresenter.GoUp();
        }
    }
}
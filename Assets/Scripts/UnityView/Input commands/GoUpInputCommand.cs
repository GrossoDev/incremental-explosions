namespace IE.UnityView
{
    public class GoUpInputCommand : PlayerInputCommand
    {
        protected override void OnKeyDown()
        {
            playerPresenter.GoUp();
        }
    }
}
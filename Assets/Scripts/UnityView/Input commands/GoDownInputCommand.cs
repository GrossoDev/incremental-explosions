namespace IE.UnityView
{
    public class GoDownInputCommand : PlayerInputCommand
    {
        protected override void OnKeyDown()
        {
            playerPresenter.GoDown();
        }
    }
}
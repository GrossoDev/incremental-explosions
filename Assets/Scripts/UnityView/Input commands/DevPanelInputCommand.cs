namespace IE.UnityView
{
    public class DevPanelInputCommand : GameInputCommand
    {
        protected override void OnKeyDown()
        {
            gameView.ToggleDevPanel();
        }
    }
}
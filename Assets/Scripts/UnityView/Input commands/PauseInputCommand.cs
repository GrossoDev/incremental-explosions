namespace IE.UnityView
{
    public class PauseInputCommand : GameInputCommand
    {
        protected override void OnKeyUp()
        {
            gamePresenter.TogglePause();
        }
    }
}
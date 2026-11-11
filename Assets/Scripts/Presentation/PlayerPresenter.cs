using IE.Model;
using IE.UnityView;

namespace IE.Presentation
{
    public class PlayerPresenter : EntityPresenter
    {
        public PlayerPresenter(Entity model) : base(model)
        {
        }

        public void GoUp()
        {
            GetModel().MoveInDirection(GridDirection.Up);
        }

        public void GoDown()
        {
            GetModel().MoveInDirection(GridDirection.Down);
        }

        public void GoLeft()
        {
            GetModel().MoveInDirection(GridDirection.Left);
        }

        public void GoRight()
        {
            GetModel().MoveInDirection(GridDirection.Right);
        }

        public Player GetModel()
        {
            return (Player)model;
        }
    }
}

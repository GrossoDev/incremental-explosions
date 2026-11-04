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
            model.MoveInDirection(GridDirection.Up);
        }

        public void GoDown()
        {
            model.MoveInDirection(GridDirection.Down);
        }

        public void GoLeft()
        {
            model.MoveInDirection(GridDirection.Left);
        }

        public void GoRight()
        {
            model.MoveInDirection(GridDirection.Right);
        }
    }
}

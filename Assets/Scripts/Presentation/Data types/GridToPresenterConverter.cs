using IE.Model;

namespace IE.Presentation
{
    public class GridToPresenterConverter
    {
        public PresenterPosition ToPresenterPosition(GridPosition gridPosition)
        {
            return new PresenterPosition(gridPosition.X, gridPosition.Y);
        }

        public GridPosition ToGridPosition(PresenterPosition presenterPosition)
        {
            return new GridPosition(presenterPosition.X, presenterPosition.Y);
        }

        public PresenterDirection ToPresenterDirection(GridDirection gridDirection)
        {
            return new PresenterDirection(gridDirection.DeltaX, gridDirection.DeltaY);
        }

        public GridDirection ToGridDirection(PresenterDirection presenterDirection)
        {
            return new GridDirection(presenterDirection.DeltaX, presenterDirection.DeltaY);
        }
    }
}
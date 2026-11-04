using IE.Presentation;

namespace IE.Model
{
    public abstract class Entity
    {
        protected Cell occupiedCell;
        protected GridDirection direction;
        protected EntityPresenter presenter;

        protected Entity(Cell cell, GridDirection direction)
        {
            this.occupiedCell = cell;
            this.direction = direction;
            CreatePresenter();
        }

        public Arena GetArena()
        {
            return occupiedCell.GetArena();
        }

        public Cell GetCell()
        {
            return occupiedCell;
        }

        public GridPosition GetPosition()
        {
            return occupiedCell.GetPosition();
        }

        public GridDirection GetDirection()
        {
            return direction;
        }

        public void SetDirection(GridDirection newDirection)
        {
            direction = newDirection;
            presenter.UpdateDirection(direction);
        }

        public void MoveToCell(Cell targetCell)
        {
            Cell currentCell = GetCell();

            currentCell.RemoveEntity(this);
            targetCell.AddEntity(this);

            ITouchable[] touchedEntities = targetCell.GetEntitiesOfType<ITouchable>();

            foreach (ITouchable entitity in touchedEntities)
            {
                entitity.Touch(this);
            }

            occupiedCell = targetCell;
            presenter.UpdatePosition(targetCell.GetPosition());
        }

        public void MoveInDirection(GridDirection direction)
        {
            Arena arena = GetArena();
            Cell currentCell = GetCell();

            SetDirection(direction);
            
            if (arena.DoesCellHaveNeighbor(currentCell, direction))
            {
                Cell targetCell = arena.GetCellNeighbor(currentCell, direction);

                MoveToCell(targetCell);
            }
        }

        public void DestroyEntity()
        {
            occupiedCell.RemoveEntity(this);
            presenter.EntityDestroyed();
        }

        protected virtual void CreatePresenter()
        {
            this.presenter = new EntityPresenter(this);
            presenter.UpdatePosition(occupiedCell.GetPosition());
            presenter.UpdateDirection(direction);
        }
    }
}

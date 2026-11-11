using System;
using System.Collections.Generic;
using System.Linq;
using IE.Presentation;

namespace IE.Model
{
    public abstract class Entity
    {
        protected Cell occupiedCell;
        protected GridDirection direction;
        protected EntityPresenter presenter;
        protected CollisionHandlerBase collisionHandler;

        protected Entity(Cell cell, GridDirection direction, CollisionHandlerBase collisionHandler)
        {
            this.occupiedCell = cell;
            this.direction = direction;
            this.collisionHandler = collisionHandler;

            cell.AddEntity(this);
            CreatePresenter();
            InteractWithOtherEntitiesInSameCell();
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
            occupiedCell = targetCell;
            presenter.UpdatePosition(targetCell.GetPosition());

            InteractWithOtherEntitiesInSameCell();
        }

        public void MoveInDirection(GridDirection direction)
        {
            var arena = GetArena();
            var currentCell = GetCell();

            SetDirection(direction);
            
            if (arena.HasNeighbourCell(currentCell, direction))
            {
                var targetCell = arena.GetCellNeighbour(currentCell, direction);

                MoveToCell(targetCell);
            }
        }

        private void InteractWithOtherEntitiesInSameCell()
        {
            var currentCell = GetCell();
            var otherEntities = currentCell.GetAllEntities().Where(entity => entity != this);

            collisionHandler.DoCollisionsBetween(this, otherEntities);
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

        public virtual void Tick() {}
    }
}

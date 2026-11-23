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
        protected long ticksElapsedSinceSpawn;
        protected CollisionHandlerBase collisionHandler;
        private List<OneshotAction> oneshotActions = new List<OneshotAction>();
        private List<RecurringAction> recurringActions = new List<RecurringAction>();

        protected Entity(Cell cell, GridDirection direction, CollisionHandlerBase collisionHandler)
        {
            this.occupiedCell = cell;
            this.direction = direction;
            this.collisionHandler = collisionHandler;
            this.ticksElapsedSinceSpawn = 0;

            cell.AddEntity(this);
            GetArena().EntityCreated(this);
            CreatePresenter();
            
            CollideOtherEntitiesInSameCell();
        }

        public virtual void Tick()
        {
            var currentOneshotActions = oneshotActions.ToArray();

            foreach (var oneshotAction in currentOneshotActions)
            {
                long callDue = oneshotAction.ticksBeforeCall + oneshotAction.createdAt;

                if (callDue < ticksElapsedSinceSpawn)
                {
                    oneshotAction.action.Invoke();
                    oneshotActions.Remove(oneshotAction);
                }
            }

            foreach (var recurringAction in recurringActions)
            {
                long nextCallDue = recurringAction.ticksBetweenCalls + recurringAction.lastCall;

                if (nextCallDue < ticksElapsedSinceSpawn)
                {
                    recurringAction.action.Invoke();
                    recurringAction.lastCall = ticksElapsedSinceSpawn;
                }
            }

            ticksElapsedSinceSpawn++;
        }

        public void MoveToCell(Cell targetCell)
        {
            Cell currentCell = GetCell();

            currentCell.RemoveEntity(this);
            targetCell.AddEntity(this);
            occupiedCell = targetCell;
            presenter.UpdatePosition(targetCell.GetPosition());

            CollideOtherEntitiesInSameCell();
        }

        private void CollideOtherEntitiesInSameCell()
        {
            var currentCell = GetCell();
            var otherEntities = currentCell.GetAllEntities().Where(entity => entity != this);

            collisionHandler.DoCollisionsBetween(this, otherEntities);
        }

        public void RegisterRecurringAction(int ticksBetweenCalls, Action action)
        {
            RecurringAction newRecurringAction = new RecurringAction()
            {
                ticksBetweenCalls = ticksBetweenCalls,
                action = action,
                lastCall = ticksElapsedSinceSpawn
            };

            recurringActions.Add(newRecurringAction);
        }

        public void RegisterOneshotAction(int ticksBeforeCall, Action action)
        {
            OneshotAction newOneshotAction = new OneshotAction()
            {
                ticksBeforeCall = ticksBeforeCall,
                action = action,
                createdAt = ticksElapsedSinceSpawn
            };

            oneshotActions.Add(newOneshotAction);
        }

        public void DestroyEntity()
        {
            occupiedCell.RemoveEntity(this);
            GetArena().EntityDestroyed(this);
            presenter.EntityDestroyed();
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

        protected virtual void CreatePresenter()
        {
            this.presenter = new EntityPresenter(this);
            presenter.UpdatePosition(occupiedCell.GetPosition());
            presenter.UpdateDirection(direction);
        }

        class RecurringAction
        {
            public int ticksBetweenCalls;
            public long lastCall;
            public Action action;
        }

        class OneshotAction
        {
            public int ticksBeforeCall;
            public long createdAt;
            public Action action;
        }
    }
}

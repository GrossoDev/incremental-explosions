using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.Model
{
    public abstract class Enemy : Entity
    {
        protected int maxHealth;
        protected int remainingHealth;

        protected Enemy(Cell cell, GridDirection direction, CollisionHandlerBase collisionHandler) : base(cell, direction, collisionHandler)
        {
            this.maxHealth = 10;
            this.remainingHealth = maxHealth;
        }

        public virtual void Hurt(int damage)
        {
            remainingHealth = Math.Max(0, remainingHealth - damage);

            if (remainingHealth <= 0)
            {
                Die();
            }
        }

        public void MoveInDirection(GridDirection direction)
        {
            var arena = GetArena();
            var currentCell = GetCell();

            SetDirection(direction);
            
            if (arena.HasNeighbourCell(currentCell, direction))
            {   
                var targetCell = arena.GetCellNeighbour(currentCell, direction);

                if (DoesCellHaveEnemies(targetCell))
                    return;

                MoveToCell(targetCell);
            }
        }

        public void MoveTowards(GridPosition targetPosition)
        {
            GridPosition currentPosition = GetPosition();

            int dx = targetPosition.X - currentPosition.X;
            int dy = targetPosition.Y - currentPosition.Y;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                MoveInDirection(dx > 0 ? GridDirection.Right : GridDirection.Left);
            }
            else if (dy != 0)
            {
                MoveInDirection(dy > 0 ? GridDirection.Up : GridDirection.Down);
            }
        }

        public void MoveRandomly()
        {
            List<GridDirection> availableDirections = DirectionsWithNoEnemies();
            
            if (availableDirections.Count > 0)
            {
                GridDirection randomDirection = RandomUtils.GetRandomItem(availableDirections);
                MoveInDirection(randomDirection);
            }
        }

        protected bool DoesCellHaveEnemies(Cell cell)
        {
            var enemiesInNeighbourCell = cell.GetEntitiesOfType<Enemy>();
                    
            return enemiesInNeighbourCell.Any();
        }

        protected List<GridDirection> DirectionsWithNoEnemies()
        {
            Arena arena = GetArena();
            Cell currentCell = GetCell();
            List<GridDirection> freeDirections = new List<GridDirection>();
            List<GridDirection> searchDirections = new List<GridDirection>()
            {
                GridDirection.Up,
                GridDirection.Down,
                GridDirection.Left,
                GridDirection.Right
            };

            foreach (var direction in searchDirections)
            {
                if (arena.HasNeighbourCell(currentCell, direction))
                {
                    Cell neighbourCell = arena.GetCellNeighbour(currentCell, direction);
                    
                    if (!DoesCellHaveEnemies(neighbourCell))
                        freeDirections.Add(direction);
                }
            }

            return freeDirections;
        }

        public abstract void Die();
    }
}

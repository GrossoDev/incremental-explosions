using System;
using System.Collections.Generic;

namespace IE.Model
{
    public abstract class Enemy : Entity
    {
        protected int maxHealth;
        protected int remainingHealth;

        protected Enemy(Cell cell, GridDirection direction) : base(cell, direction)
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

        protected bool DoesCellHaveEnemies(Cell cell)
        {
            Enemy[] enemiesInNeighbourCell = cell.GetEntitiesOfType<Enemy>();
                    
            return enemiesInNeighbourCell.Length != 0;
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

using System;
using System.Collections.Generic;

namespace IE.Model
{
    public class Zombie : Enemy
    {
        private int ticksBetweenMoves = 50;
        private int ticksSinceLastMove = 0;
        
        public Zombie(Cell cell, GridDirection direction) : base(cell, direction) {}

        public override void Tick()
        {
            MoveRandomly();
        }

        public override void Die()
        {
            DestroyEntity();
        }

        private void MoveRandomly()
        {
            if (ticksSinceLastMove >= ticksBetweenMoves)
            {
                List<GridDirection> availableDirections = DirectionsWithNoEnemies();
                
                if (availableDirections.Count > 0)
                {
                    GridDirection randomDirection = RandomUtils.GetRandomItem(availableDirections);
                    MoveInDirection(randomDirection);
                }

                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }
    }
}

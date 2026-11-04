using System;
using System.Collections.Generic;
using System.Linq;

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

    public class Skeleton : Enemy
    {
        private int ticksBetweenMoves = 20;
        private int ticksSinceLastMove = 0;
        
        public Skeleton(Cell cell, GridDirection direction) : base(cell, direction) {}

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
                    List<GridDirection> longerDirections = DirectionsAwayFromPlayer();
                    List<GridDirection> targetDirections = availableDirections.Intersect(longerDirections).ToList();

                    if (targetDirections.Count > 0)
                    {
                        availableDirections = targetDirections;
                        GridDirection randomDirection = RandomUtils.GetRandomItem(availableDirections);
                        MoveInDirection(randomDirection);
                    }

                }

                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }

        private List<GridDirection> DirectionsAwayFromPlayer()
        {
            List<GridDirection> directionsAway = new List<GridDirection>();
            Cell currentCell = GetCell();
            Arena arena = GetArena();

            Cell playerCell = Player.Instance.GetCell();
            GridPosition playerPosition = playerCell.GetPosition();
            GridPosition skeletonPosition = currentCell.GetPosition();

            int deltaX = skeletonPosition.X - playerPosition.X;
            int deltaY = skeletonPosition.Y - playerPosition.Y;

            if (deltaX > 0 && arena.DoesCellHaveNeighbor(currentCell, GridDirection.Right))
                directionsAway.Add(GridDirection.Right);
            else if (deltaX < 0 && arena.DoesCellHaveNeighbor(currentCell, GridDirection.Left))
                directionsAway.Add(GridDirection.Left);

            if (deltaY > 0 && arena.DoesCellHaveNeighbor(currentCell, GridDirection.Up))
                directionsAway.Add(GridDirection.Up);
            else if (deltaY < 0 && arena.DoesCellHaveNeighbor(currentCell, GridDirection.Down))
                directionsAway.Add(GridDirection.Down);

            return directionsAway;
        }
    }
}

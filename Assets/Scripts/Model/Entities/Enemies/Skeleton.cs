using System.Collections.Generic;
using System.Linq;

namespace IE.Model
{
    public class Skeleton : Enemy
    {
        private int ticksBetweenMoves = 20;
        private int ticksSinceLastMove = 0;
        
        public Skeleton(Cell cell, GridDirection direction) : base(cell, direction, new DefaultCollisionHandler())
        {
            this.maxHealth = 5;
        }

        public override void Tick()
        {
            if (ticksSinceLastMove >= ticksBetweenMoves)
            {
                MoveAwayFromPlayer();
                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }

        public override void Die()
        {
            DestroyEntity();
        }

        private void MoveAwayFromPlayer()
        {
            List<GridDirection> directionsWithNoEnemies = DirectionsWithNoEnemies();
            
            if (directionsWithNoEnemies.Count == 0)
                return;

            List<GridDirection> directionsAwayFromPlayer = DirectionsAwayFromPlayer();
            List<GridDirection> bestDirections = directionsAwayFromPlayer.Intersect(directionsWithNoEnemies).ToList();
            GridDirection targetDirection;

            if (bestDirections.Count > 0)
                targetDirection = RandomUtils.GetRandomItem(bestDirections);
            else
                targetDirection = RandomUtils.GetRandomItem(directionsWithNoEnemies);

            MoveInDirection(targetDirection);
        }

        private List<GridDirection> DirectionsAwayFromPlayer()
        {
            Arena arena = GetArena();
            Cell currentCell = GetCell();
            List<GridDirection> directionsAway = new List<GridDirection>();

            Cell playerCell = Game.Instance.GetPlayer().GetCell();
            GridPosition playerPosition = playerCell.GetPosition();
            GridPosition skeletonPosition = currentCell.GetPosition();

            int deltaX = skeletonPosition.X - playerPosition.X;
            int deltaY = skeletonPosition.Y - playerPosition.Y;

            if (deltaX > 0 && arena.HasNeighbourCell(currentCell, GridDirection.Right))
                directionsAway.Add(GridDirection.Right);
            else if (deltaX < 0 && arena.HasNeighbourCell(currentCell, GridDirection.Left))
                directionsAway.Add(GridDirection.Left);

            if (deltaY > 0 && arena.HasNeighbourCell(currentCell, GridDirection.Up))
                directionsAway.Add(GridDirection.Up);
            else if (deltaY < 0 && arena.HasNeighbourCell(currentCell, GridDirection.Down))
                directionsAway.Add(GridDirection.Down);

            return directionsAway;
        }
    }
}

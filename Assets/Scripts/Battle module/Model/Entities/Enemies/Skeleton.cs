using System.Collections.Generic;
using System.Linq;

namespace Battle.Model
{
    public class Skeleton : Enemy
    {
        private int ticksBetweenMoves = 22;
        
        public Skeleton(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 15;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 30;

            RegisterRecurringAction(ticksBetweenMoves, MoveAwayFromPlayer);
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
            Player player = arena.FindNearestEntity<Player>(currentCell);
            List<GridDirection> directionsAway = new List<GridDirection>();

            GridPosition playerPosition = player.GetPosition();
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

namespace IE.Model
{
    public class Fire : Entity
    {
        private int damage = 10;
        private int durationInTicks = 10;
        private int remainingRadius = 1;
        private bool hasExpanded = false;

        public Fire(Cell cell, GridDirection direction, int damage, int remainingRadius) : base(cell, direction, new FireCollisionHandler())
        {
            this.damage = damage;
            this.remainingRadius = remainingRadius - 1;
        }

        public int GetDamage()
        {
            return damage;
        }

        public override void Tick()
        {
            if (remainingRadius > 0 && !hasExpanded)
            {
                PropagateFire();
                hasExpanded = true;
            }
            else
            {
                if (durationInTicks <= 0)
                    DestroyEntity();

                durationInTicks--;
            }
        }

        private void PropagateFire()
        {
            GridDirection propagationDirection = GetDirection();

            if (propagationDirection == GridDirection.None)
            {
                ExpandFireInDirection(GridDirection.Up);
                ExpandFireInDirection(GridDirection.Down);
                ExpandFireInDirection(GridDirection.Left);
                ExpandFireInDirection(GridDirection.Right);
            }
            else
            {
                ExpandFireInDirection(propagationDirection);
            }
        }

        private void ExpandFireInDirection(GridDirection propagationDirection)
        {
            Cell currentCell = GetCell();
            Arena arena = currentCell.GetArena();

            if (arena.HasNeighbourCell(currentCell, propagationDirection))
            {
                Cell neighborCell = arena.GetCellNeighbour(currentCell, propagationDirection);
                Fire newFire = new Fire(neighborCell, propagationDirection, damage, remainingRadius);
            }
        }
    }
}

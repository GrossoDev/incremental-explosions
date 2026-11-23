namespace IE.Model
{
    public class Fire : Entity
    {
        private int damage = 10;
        private int lifetimeInTicks = 10;
        private int remainingRadius = 1;
        private int propagationDelayInTicks;

        public Fire(Cell cell, GridDirection direction, int damage, int remainingRadius) : base(cell, direction, new FireCollisionHandler())
        {
            this.damage = damage;
            this.remainingRadius = remainingRadius - 1;

            RegisterOneshotAction(propagationDelayInTicks, PropagateFire);
            RegisterOneshotAction(lifetimeInTicks, DestroyEntity);
        }

        public int GetDamage()
        {
            return damage;
        }

        private void PropagateFire()
        {
            if (remainingRadius <= 0) return;

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

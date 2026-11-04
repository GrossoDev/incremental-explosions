namespace IE.Model
{
    public class Fire : Entity, ITickable
    {
        private int damage = 1;
        private int durationInTicks = 10;
        private int remainingRadius = 1;
        private bool hasExpanded = false;

        public Fire(Cell cell, GridDirection direction, int damage, int remainingRadius) : base(cell, direction)
        {
            this.damage = damage;
            this.remainingRadius = remainingRadius - 1;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void Tick()
        {
            BurnOtherEntitiesInCell();

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

            if (arena.DoesCellHaveNeighbor(currentCell, propagationDirection))
            {
                Cell neighborCell = arena.GetCellNeighbor(currentCell, propagationDirection);
                Fire newFire = new Fire(neighborCell, propagationDirection, damage, remainingRadius);
                neighborCell.AddEntity(newFire);
            }
        }

        private void BurnOtherEntitiesInCell()
        {
            Cell cellOnFire = GetCell();
            IHurtable[] burntEntities = cellOnFire.GetEntitiesOfType<IHurtable>();

            foreach (var entity in burntEntities)
            {
                entity.Hurted(this, damage);
            }
        }
    }
}

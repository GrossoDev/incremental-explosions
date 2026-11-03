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
                ExpandFire();
                hasExpanded = true;
            }
            else
            {
                if (durationInTicks <= 0)
                    DestroyEntity();

                durationInTicks--;
            }
        }

        private void ExpandFire()
        {
            Cell currentCell = GetCell();
            Arena arena = currentCell.GetArena();
            GridDirection expansionDirection = GetDirection();

            if (arena.DoesCellHaveNeighbor(currentCell, expansionDirection))
            {
                Cell neighborCell = arena.GetCellNeighbor(currentCell, expansionDirection);
                Fire newFire = new Fire(neighborCell, expansionDirection, damage, remainingRadius);
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

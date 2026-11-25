namespace Battle.Model
{
    public class AlienUndercover : Enemy
    {
        private int ticksBetweenMoves = 20;

        public AlienUndercover(Cell cell, GridDirection direction) : base(cell, direction, new AlienUndercoverCollisionHandler())
        {
            this.maxHealth = 10;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 100;

            RegisterRecurringAction(ticksBetweenMoves, MoveTowardsCoin);
        }

        public void CollectedCoin(int value)
        {
            this.valueOnDeath += value / 2;
        }

        private void MoveTowardsCoin()
        {
            Arena arena = GetArena();
            Cell currentCell = GetCell();

            if (arena.AreThereAnyEntitiesOfType<Coin>())
            {
                Coin coin = arena.FindNearestEntity<Coin>(currentCell);
                GridPosition coinPosition = coin.GetPosition();

                MoveTowards(coinPosition);
            }
            else
            {
                MoveRandomly();
            }
        }
    }
}


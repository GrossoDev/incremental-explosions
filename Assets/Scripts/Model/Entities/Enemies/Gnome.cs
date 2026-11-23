using System;

namespace IE.Model
{
    public class Gnome : Enemy
    {
        private int ticksBetweenMoves = 20;
        
        public Gnome(Cell cell, GridDirection direction) : base(cell, direction, new GnomeCollisionHandler())
        {
            this.maxHealth = 10;
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

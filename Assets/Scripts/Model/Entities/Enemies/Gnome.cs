using System;

namespace IE.Model
{
    public class Gnome : Enemy
    {
        private int ticksBetweenMoves = 20;
        private int ticksSinceLastMove = 0;
        private int stolenCoins = 0;
        
        public Gnome(Cell cell, GridDirection direction) : base(cell, direction, new GnomeCollisionHandler())
        {
            this.maxHealth = 10;
            this.valueOnDeath = 100;
        }

        public override void Tick()
        {
            if (ticksSinceLastMove >= ticksBetweenMoves)
            {
                MoveTowardsCoin();
                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }

        public void CollectedCoin(int value)
        {
            stolenCoins += value;
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

namespace Battle.Model
{
    public class WereWolf : Enemy
    {
        private int ticksBetweenMoves = 15;

        public WereWolf(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 20;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 220;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


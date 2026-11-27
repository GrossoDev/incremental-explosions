namespace Battle.Model
{
    public class WereWolf : Enemy
    {
        private int ticksBetweenMoves = 18;

        public WereWolf(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 65;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 90;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


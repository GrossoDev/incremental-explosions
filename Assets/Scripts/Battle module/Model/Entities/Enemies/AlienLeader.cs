namespace Battle.Model
{
    public class AlienLeader : Enemy
    {
        private int ticksBetweenMoves = 24;

        public AlienLeader(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 200;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 180;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


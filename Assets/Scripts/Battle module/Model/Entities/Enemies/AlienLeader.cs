namespace Battle.Model
{
    public class AlienLeader : Enemy
    {
        private int ticksBetweenMoves = 25;

        public AlienLeader(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 60;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 450;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


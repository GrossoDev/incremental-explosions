namespace Battle.Model
{
    public class AlienGrunt : Enemy
    {
        private int ticksBetweenMoves = 28;

        public AlienGrunt(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 110;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 140;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


namespace Battle.Model
{
    public class AlienGrunt : Enemy
    {
        private int ticksBetweenMoves = 30;

        public AlienGrunt(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 35;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 350;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


namespace Battle.Model
{
    public class Latina : Enemy
    {
        private int ticksBetweenMoves = 30;

        public Latina(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 300;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 0;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


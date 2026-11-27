namespace Battle.Model
{
    public class Orc : Enemy
    {
        private int ticksBetweenMoves = 55;

        public Orc(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 55;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 60;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


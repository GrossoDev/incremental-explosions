namespace Battle.Model
{
    public class Orc : Enemy
    {
        private int ticksBetweenMoves = 60;

        public Orc(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 30;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 150;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


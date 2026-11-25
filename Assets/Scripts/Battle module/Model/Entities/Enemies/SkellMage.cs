namespace Battle.Model
{
    public class SkellMage : Enemy
    {
        private int ticksBetweenMoves = 35;

        public SkellMage(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 15;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 180;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


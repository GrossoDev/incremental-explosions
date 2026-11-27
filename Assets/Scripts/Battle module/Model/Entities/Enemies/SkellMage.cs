namespace Battle.Model
{
    public class SkellMage : Enemy
    {
        private int ticksBetweenMoves = 32;

        public SkellMage(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 40;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 70;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


namespace Battle.Model
{
    public class SkellWarrior : Enemy
    {
        private int ticksBetweenMoves = 50;

        public SkellWarrior(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 10;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 50;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


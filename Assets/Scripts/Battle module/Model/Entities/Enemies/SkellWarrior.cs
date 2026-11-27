namespace Battle.Model
{
    public class SkellWarrior : Enemy
    {
        private int ticksBetweenMoves = 45;

        public SkellWarrior(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 10;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 20;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


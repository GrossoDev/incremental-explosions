namespace Battle.Model
{
    public class Minotaur : Enemy
    {
        private int ticksBetweenMoves = 50;

        public Minotaur(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 140;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 120;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


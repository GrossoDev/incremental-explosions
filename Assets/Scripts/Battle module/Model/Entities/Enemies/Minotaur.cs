namespace Battle.Model
{
    public class Minotaur : Enemy
    {
        private int ticksBetweenMoves = 55;

        public Minotaur(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 50;
            this.remainingHealth = maxHealth;
            this.valueOnDeath = 300;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}


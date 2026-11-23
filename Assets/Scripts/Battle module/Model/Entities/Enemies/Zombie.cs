using System;
using System.Collections.Generic;

namespace Battle.Model
{
    public class Zombie : Enemy
    {
        private int ticksBetweenMoves = 50;
        
        public Zombie(Cell cell, GridDirection direction) : base(cell, direction, new EnemyCollisionHandler())
        {
            this.maxHealth = 10;
            this.valueOnDeath = 50;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}

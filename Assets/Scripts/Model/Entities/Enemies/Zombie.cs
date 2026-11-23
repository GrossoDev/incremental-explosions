using System;
using System.Collections.Generic;

namespace IE.Model
{
    public class Zombie : Enemy
    {
        private int ticksBetweenMoves = 50;
        
        public Zombie(Cell cell, GridDirection direction) : base(cell, direction, new DefaultCollisionHandler())
        {
            this.maxHealth = 10;
            this.valueOnDeath = 50;

            RegisterRecurringAction(ticksBetweenMoves, MoveRandomly);
        }
    }
}

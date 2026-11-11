using System;
using System.Collections.Generic;

namespace IE.Model
{
    public class Zombie : Enemy
    {
        private int ticksBetweenMoves = 50;
        private int ticksSinceLastMove = 0;
        
        public Zombie(Cell cell, GridDirection direction) : base(cell, direction, new DefaultCollisionHandler())
        {
            this.maxHealth = 10;
        }

        public override void Tick()
        {
            if (ticksSinceLastMove >= ticksBetweenMoves)
            {
                MoveRandomly();
                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }

        public override void Die()
        {
            DestroyEntity();
        }
    }
}

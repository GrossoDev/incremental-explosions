using System;

namespace IE.Model
{
    public abstract class Enemy : Entity, ITickable, IHurtable
    {
        private int maxHealth;
        private int remainingHealth;

        protected Enemy(Cell cell, GridDirection direction) : base(cell, direction)
        {
            this.maxHealth = 10;
            this.remainingHealth = maxHealth;
        }

        public virtual void Hurted(Entity attacker, int damage)
        {
            remainingHealth = Math.Max(0, remainingHealth - damage);

            if (remainingHealth <= 0)
            {
                Die();
            }
        }
        public abstract void Tick();
        public abstract void Die();
    }

    public class Zombie : Enemy
    {
        private int ticksBetweenMoves = 50;
        private int ticksSinceLastMove = 0;
        
        public Zombie(Cell cell, GridDirection direction) : base(cell, direction) {}

        public override void Tick()
        {
            MoveRandomly();
        }

        public override void Die()
        {
            DestroyEntity();
        }

        private void MoveRandomly()
        {
            if (ticksSinceLastMove >= ticksBetweenMoves)
            {
                GridDirection randomDirection = RandomUtils.GetRandomDirection();
                MoveInDirection(randomDirection);

                ticksSinceLastMove = 0;
            }
            else
            {
                ticksSinceLastMove++;
            }
        }
    }
}

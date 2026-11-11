using System.Collections.Generic;

namespace IE.Model
{
    public abstract class CollisionHandlerBase
    {
        public void DoCollisionsBetween(Entity a, IEnumerable<Entity> otherEntities)
        {
            foreach (var b in otherEntities)
            {
                DoCollisionBetween(a, b);
            }
        }

        public void DoCollisionBetween(Entity a, Entity b)
        {
            dynamic concreteA = a;
            dynamic concreteB = b;
            dynamic concreteThis = this;
            
            concreteThis.HandleCollision(concreteA, concreteB);
        }
        
        public abstract void HandleCollision(Entity a, Entity b);
    }
}
namespace IE.Model
{
    public class FireCollisionHandler : CollisionHandlerBase
    { 
        public void HandleCollision(Fire fire, Enemy enemy)
        {
            int damage = fire.GetDamage();
            enemy.Hurt(damage);
        }

        public override void HandleCollision(Entity a, Entity b) { }
    }
}
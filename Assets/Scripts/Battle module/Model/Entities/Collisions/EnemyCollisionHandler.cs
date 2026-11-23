namespace Battle.Model
{
    public class EnemyCollisionHandler : CollisionHandlerBase
    { 
        public void HandleCollision(Enemy enemy, Fire fire)
        {
            int damage = fire.GetDamage();
            enemy.Hurt(damage);
        }

        public override void HandleCollision(Entity a, Entity b) { }
    }
}
namespace IE.Model
{
    public class EnemyTouchedFire : Interaction<Enemy, Fire>
    {
        public override void Interacted(Enemy enemy, Fire fire)
        {
            int damage = fire.GetDamage();
            enemy.Hurt(damage);
        }
    }

    public class FireTouchedEnemy : Interaction<Fire, Enemy>
    {
        public override void Interacted(Fire fire, Enemy enemy)
        {
            int damage = fire.GetDamage();
            enemy.Hurt(damage);
        }
    }
}
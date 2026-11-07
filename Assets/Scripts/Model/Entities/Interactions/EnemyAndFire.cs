namespace IE.Model
{
    public class EnemyAndFireInteraction : Interaction<Enemy, Fire>
    {
        public override void Interacted(Enemy enemy, Fire fire)
        {
            int damage = fire.GetDamage();
            enemy.Hurted(fire, damage);
        }
    }

    public class FireAndEnemyInteraction : Interaction<Fire, Enemy>
    {
        public override void Interacted(Fire fire, Enemy enemy)
        {
            int damage = fire.GetDamage();
            enemy.Hurted(fire, damage);
        }
    }
}
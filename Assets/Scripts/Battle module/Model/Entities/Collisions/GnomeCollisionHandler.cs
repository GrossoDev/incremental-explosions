namespace Battle.Model
{
    public class GnomeCollisionHandler : EnemyCollisionHandler
    { 
        public void HandleCollision(Gnome gnome, Coin coin)
        {
            int value = coin.GetValue();
            gnome.CollectedCoin(value);
            coin.DestroyEntity();
        }
    }
}
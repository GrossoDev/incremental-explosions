namespace IE.Model
{
    public class GnomeCollisionHandler : CollisionHandlerBase
    { 
        public void HandleCollision(Gnome gnome, Coin coin)
        {
            int value = coin.GetValue();
            gnome.CollectedCoin(value);
            coin.DestroyEntity();
        }

        public override void HandleCollision(Entity a, Entity b) { }
    }
}
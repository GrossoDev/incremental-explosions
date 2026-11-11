namespace IE.Model
{
    public class PlayerCollisionHandler : CollisionHandlerBase
    { 
        public void HandleCollision(Player player, Coin coin)
        {
            int value = coin.GetValue();
            player.CollectedCoin(value);
            coin.DestroyEntity();
        }

        public override void HandleCollision(Entity a, Entity b) { }
    }
}
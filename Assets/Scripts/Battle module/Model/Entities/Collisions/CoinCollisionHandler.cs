namespace Battle.Model
{
    public class CoinCollisionHandler : CollisionHandlerBase
    { 
        public void HandleCollision(Coin coin, Player player)
        {
            int value = coin.GetValue();
            player.CollectedCoin(value);
            coin.DestroyEntity();
        }

        public override void HandleCollision(Entity a, Entity b) { }
    }
}
namespace Battle.Model
{
    public class AlienUndercoverCollisionHandler : EnemyCollisionHandler
    {
        public void HandleCollision(AlienUndercover alien, Coin coin)
        {
            int value = coin.GetValue();
            alien.CollectedCoin(value);
            coin.DestroyEntity();
        }
    }
}


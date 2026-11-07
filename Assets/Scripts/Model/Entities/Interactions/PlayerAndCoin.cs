namespace IE.Model
{
    public class PlayerAndCoinInteraction : Interaction<Player, Coin>
    {
        public override void Interacted(Player player, Coin coin)
        {
            int coinValue = coin.GetValue();
            player.CollectedCoin(coinValue);
            coin.DestroyEntity();
        }
    }
}
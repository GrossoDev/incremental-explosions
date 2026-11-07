using IE.Model;

namespace IE.UnityView
{
    public class DevCoinSpawner : DevEntitySpawner<Coin>
    {
        protected override void SpawnEntity(Cell cell)
        {
            new Coin(cell, GridDirection.Up, 10);
        }
    }
}
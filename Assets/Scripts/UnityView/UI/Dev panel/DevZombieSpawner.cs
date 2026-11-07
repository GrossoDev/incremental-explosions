using IE.Model;

namespace IE.UnityView
{
    public class DevZombieSpawner : DevEntitySpawner<Zombie>
    {
        protected override void SpawnEntity(Cell cell)
        {
            new Zombie(cell, GridDirection.Up);
        }
    }
}
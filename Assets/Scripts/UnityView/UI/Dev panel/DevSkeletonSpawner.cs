using IE.Model;

namespace IE.UnityView
{
    public class DevSkeletonSpawner : DevEntitySpawner<Skeleton>
    {
        protected override void SpawnEntity(Cell cell)
        {
            new Skeleton(cell, GridDirection.Up);
        }
    }
}
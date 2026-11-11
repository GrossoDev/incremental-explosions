using IE.Model;

namespace IE.UnityView
{
    public class DevGnomeSpawner : DevEntitySpawner<Gnome>
    {
        protected override void SpawnEntity(Cell cell)
        {
            new Gnome(cell, GridDirection.Up);
        }
    }
}
namespace IE.Model
{
    public class Battle
    {
        private const int Width = 40;
        private const int Height = 40;
        private Arena arena;
        private Player player;

        public void Initialize()
        {
            CreateArena();
            CreatePlayer();
        }

        public Arena GetArena()
        {
            return arena;
        }

        public Player GetPlayer()
        {
            return player;
        }

        private void CreatePlayer()
        {
            Cell playerCell = arena.GetRandomFreeCell();
            player = new Player(playerCell, GridDirection.Up);
        }

        private void CreateArena()
        {
            arena = new Arena();
            arena.Initialize(Width, Height);
        }
    }
}
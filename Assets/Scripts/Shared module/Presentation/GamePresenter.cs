using Shared.Model;

namespace Shared.Presentation
{
    public class GamePresenter
    {
        private Game model;

        public GamePresenter()
        {
            model = Game.Instance;
        }

        public int GetBattleCoins()
        {
            return model.GetBattleCoins();
        }

        public void SetBattleCoins(int amount)
        {
            model.SetBattleCoins(amount);
        }

        public int GetTotalCoins()
        {
            return model.GetTotalCoins();
        }

        public void SetTotalCoins(int amount)
        {
            model.SetTotalCoins(amount);
        }

        public int GetCurrentLevel()
        {
            return model.GetCurrentLevel();
        }

        public void SetCurrentLevel(int level)
        {
            model.SetCurrentLevel(level);
        }

        public void StartNewGame()
        {
            model.NewGame();
        }

        public string Serialize()
        {
            return model.Serialize();
        }

        public void Deserialize(string serializedGame)
        {
            model.Deserialize(serializedGame);
        }
    }
}
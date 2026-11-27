using System.Threading.Tasks;
using UnityEngine;

namespace Shared.View
{
    public class MockGameView : GameView
    {
        [SerializeField] [Range(1,50)] private int fakeLevel = 1;
        [SerializeField] [Min(0)] private int fakeRoundCoins = 0;
        [SerializeField] [Min(0)] private int fakeTotalCoins = 0;
        [SerializeField] private string language = "en";

        private void OnEnable()
        {
            StartNewGame();
            gamePresenter.SetBattleCoins(fakeRoundCoins);
            gamePresenter.SetTotalCoins(fakeTotalCoins);
            gamePresenter.SetCurrentLevel(fakeLevel);
            SetLang().Wait();
        }

        private Task SetLang()
        {
            Task setLanguageTask = Localization.Instance.SetLanguageAsync(language);

            return setLanguageTask;
        }
    }
}
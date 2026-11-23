using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class ArenaView : MonoBehaviour
    {
        private ArenaPresenter presenter;

        public void SetPresenter(ArenaPresenter arenaPresenter)
        {
            presenter = arenaPresenter;
        }
    }
}
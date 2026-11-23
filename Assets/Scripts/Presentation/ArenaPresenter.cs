using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class ArenaPresenter
    {
        private Arena model;
        private ArenaView view;

        public ArenaPresenter(Arena arena)
        {
            this.model = arena;
            LoadView();
        }

        private void LoadView()
        {
            string prefabName = "Arena";
            string prefabPath = $"Prefabs/{prefabName}";

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject viewObject = GameObject.Instantiate(prefab);
            view = viewObject.GetComponent<ArenaView>();
            view.SetPresenter(this);
        }
    }
}
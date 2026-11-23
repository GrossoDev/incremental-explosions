using System;
using Battle.Model;
using Battle.View;
using UnityEngine;

namespace Battle.Presentation
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

        public void Initialize()
        {
            view.Initialize();
        }

        public int GetWidth()
        {
            return model.GetWidth();
        }

        public int GetHeight()
        {
            return model.GetHeight();
        }

        private void LoadView()
        {
            string prefabName = "Arena View";
            string prefabPath = $"Prefabs/Arena/{prefabName}";

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject viewObject = GameObject.Instantiate(prefab);
            view = viewObject.GetComponent<ArenaView>();
            view.SetPresenter(this);
        }
    }
}
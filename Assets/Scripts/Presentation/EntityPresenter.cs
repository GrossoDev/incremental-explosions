using System;
using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class EntityPresenter
    {
        protected Entity model;
        protected EntityView view;
        private GridToPresenterConverter gridToPresenterConverter = new GridToPresenterConverter();

        public EntityPresenter(Entity model)
        {
            this.model = model;
            LoadView();
        }

        public void Tick()
        {
            model.Tick();
        }

        public void UpdatePosition(GridPosition newPosition)
        {
            PresenterPosition presenterPosition = gridToPresenterConverter.ToPresenterPosition(newPosition);
            view.UpdatePosition(presenterPosition);
        }

        public void UpdateDirection(GridDirection newDirection)
        {
            PresenterDirection presenterDirection = gridToPresenterConverter.ToPresenterDirection(newDirection);
            view.UpdateDirection(presenterDirection);
        }

        public void EntityDestroyed()
        {
            view.EntityDestroyed();
        }

        private void LoadView()
        {
            string prefabName = model.GetType().Name;
            string prefabPath = $"Prefabs/Entities/{prefabName}";

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject viewObject = GameObject.Instantiate(prefab);
            view = viewObject.GetComponent<EntityView>();
            view.SetPresenter(this);
        }
    }
}

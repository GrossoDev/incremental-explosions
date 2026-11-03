using System;
using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class EntityPresenter
    {
        private Entity model;
        private EntityView view;
        private GridToPresenterConverter gridToPresenterConverter = new GridToPresenterConverter();

        public EntityPresenter(Entity model)
        {
            this.model = model;
            string prefabPath = $"Prefabs/Entities/{model.GetType().Name}";
            Debug.Log($"Loading prefab for {model.GetType().Name} from path: {prefabPath}");

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject viewObject = GameObject.Instantiate(prefab);
            view = viewObject.GetComponent<EntityView>();
            view.SetPresenter(this);
        }

        public void Tick()
        {
            if (model is ITickable tickable)
            {
                tickable.Tick();
            }
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
    }
}

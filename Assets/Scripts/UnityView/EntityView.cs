using System;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class EntityView : MonoBehaviour
    {
        private EntityPresenter presenter;
        private PresenterToViewConverter presenterToView = new PresenterToViewConverter();

        private void FixedUpdate()
        {
            presenter.Tick();
        }

        public void UpdatePosition(PresenterPosition newPosition)
        {
            Vector3 position = presenterToView.ToVector3(newPosition);
            this.transform.position = position;
        }

        public void UpdateDirection(PresenterDirection newDirection)
        {
            
        }

        public void EntityDestroyed()
        {
            Destroy(this.gameObject);
        }

        public void SetPresenter(EntityPresenter presenter)
        {
            this.presenter = presenter;
        }
    }
}
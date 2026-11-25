using System;
using Battle.Model;
using Battle.Presentation;
using UnityEngine;

namespace Battle.View
{
    public class EntityView : MonoBehaviour
    {
        protected EntityPresenter presenter;
        protected bool positionInitialized = false;
        protected bool rotationInitialized = false;
        protected Vector3 targetPosition;
        protected Quaternion targetRotation;
        protected PresenterToViewConverter presenterToView = new PresenterToViewConverter();

        protected virtual void Update()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 10f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        private void FixedUpdate()
        {
            presenter.Tick();
        }

        public virtual void UpdatePosition(PresenterPosition newPosition)
        {
            Vector3 position = presenterToView.ToVector3(newPosition);
            targetPosition = position;

            if (!positionInitialized)
            {
                this.transform.position = position;
                positionInitialized = true;
            }
        }

        public void UpdateDirection(PresenterDirection newDirection)
        {
            Quaternion rotation = presenterToView.ToQuaternion(newDirection);
            targetRotation = rotation;

            if (!rotationInitialized)
            {
                this.transform.rotation = rotation;
                rotationInitialized = true;
            }
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
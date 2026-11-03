using System;
using UnityEngine;

namespace IE.Presentation
{
    public class PresenterToViewConverter
    {
        public Vector3 ToVector3(PresenterPosition presenterPosition)
        {
            return new Vector3(presenterPosition.X, 0, presenterPosition.Y);
        }

        public PresenterPosition ToPresenterPosition(Vector3 viewPosition)
        {
            return new PresenterPosition((int)viewPosition.x, (int)viewPosition.z);
        }

        public Quaternion ToQuaternion(PresenterDirection presenterDirection)
        {
            Vector3 forward = new Vector3(presenterDirection.DeltaX, 0, presenterDirection.DeltaY);

            return Quaternion.LookRotation(forward);
        }

        public PresenterDirection ToPresenterDirection(Quaternion viewDirection)
        {
            throw new NotImplementedException();
        }
    }
}

using UnityEngine;
using System.Linq;

namespace IE.UnityView
{
    public abstract class InputCommand : MonoBehaviour
    {
        [SerializeField] protected KeyCode[] keys;

        protected void Update()
        {
            Check();
        }

        private void Check()
        {
            if (keys.Any(Input.GetKeyUp))
                OnKeyUp();

            if (keys.Any(Input.GetKeyDown))
                OnKeyDown();

            if (keys.Any(Input.GetKey))
                OnKeyHeld();
        }

        protected virtual void OnKeyUp() {}
        protected virtual void OnKeyDown() {}
        protected virtual void OnKeyHeld() {}
    }
}
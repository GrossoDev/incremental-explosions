using UnityEngine;
using System.Linq;
using System.Collections;

namespace Battle.View
{
    public abstract class InputCommand : MonoBehaviour
    {
        [SerializeField] protected float pollingIntervalInSeconds = 0.1f;
        [SerializeField] protected KeyCode[] keys;
        [SerializeField] private bool allowWhenPaused = false;
        private float timeSinceLastPoll = 0f;

        private void Update()
        {
            if (!allowWhenPaused && Time.timeScale <= 0f)
                return;

            Check();
        }

        private void Check()
        {
            if (keys.Any(Input.GetKeyUp))
                OnKeyUp();

            if (keys.Any(Input.GetKeyDown))
                OnKeyDown();

            if (keys.Any(Input.GetKey))
            {
                if (timeSinceLastPoll >= pollingIntervalInSeconds)
                {
                    OnKeyHeld();
                    timeSinceLastPoll = 0f;
                }
                else
                {
                    timeSinceLastPoll += Time.deltaTime;
                    return;
                }
            }
        }

        protected virtual void OnKeyUp() {}
        protected virtual void OnKeyDown() {}
        protected virtual void OnKeyHeld() {}
    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shared.View
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private SceneField nextScene;

        public void SwitchToNext()
        {
            SwitchTo(nextScene);
        }

        public void SwitchTo(SceneField targetScene)
        {
            if (targetScene == null || string.IsNullOrEmpty(targetScene.SceneName))
                throw new InvalidOperationException("Target scene is not assigned in SceneSwitcher.");

            Time.timeScale = 1f;
            SceneManager.LoadScene(targetScene);
        }
    }
}

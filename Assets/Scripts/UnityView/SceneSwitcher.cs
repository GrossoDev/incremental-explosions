using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IE.UnityView
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private SceneField nextScene;

        private AsyncOperation loadSceneOperation;

        private void Start()
        {
            loadSceneOperation = SceneManager.LoadSceneAsync(nextScene);
            loadSceneOperation.allowSceneActivation = false;
        } 

        public void SwitchToNext()
        {
            if (loadSceneOperation is null)
                throw new Exception($"Tried to switch to the next scene but we didn't even begin loading it.");
            
            loadSceneOperation.allowSceneActivation = true;
        }
    }
}
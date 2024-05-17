namespace MajorProject.Manager
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ScenesManager : MonoBehaviour
    {
        public static ScenesManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.LoadScene("90. Game UI", LoadSceneMode.Additive);
        }
    }
}
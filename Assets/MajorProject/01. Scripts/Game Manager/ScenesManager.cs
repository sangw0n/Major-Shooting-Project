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
            Instance = this;
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
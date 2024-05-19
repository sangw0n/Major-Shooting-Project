namespace MajorProject.Lobby
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene("00. Lobby");
        }
    }
}
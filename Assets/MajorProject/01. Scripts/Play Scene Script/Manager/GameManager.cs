namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Player player;

        private void Awake()
        {
            Instance = this;
        }
    }
}
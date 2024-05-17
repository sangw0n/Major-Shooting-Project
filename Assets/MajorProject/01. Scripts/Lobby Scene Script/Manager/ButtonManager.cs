namespace MajorProject.Lobby
{
    // # System
    using System.Collections;
    using System.Collections.Generic;
    using MajorProject.Manager;

    // # Unity
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class ButtonManager : MonoBehaviour
    {
        // 버튼 변수 
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button howtoplayButton;

        private void Start()
        {
            startButton.onClick.AddListener(() => OnClickStartBtn());
        }

        private void OnClickStartBtn()
        {
            ScenesManager.Instance.Load("01. Stage1");
        }
    }
}
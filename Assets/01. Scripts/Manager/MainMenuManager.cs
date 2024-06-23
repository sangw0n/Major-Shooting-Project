// # System
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEditorInternal;


// # Unity
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------

    [Header("[# Button Info]")]
    [SerializeField] private Button             startButton;

    [Space(10)]
    [SerializeField] private Button             creditButton;
    [SerializeField] private Button             creditHomeButton;
    [SerializeField] private GameObject         creditBackground;

    [Space(10)]
    [SerializeField] private Button             settingButton;
    [SerializeField] private Button             howToPlayButton;
    [SerializeField] private Button             exitButton;

    [Header("# Other Info")]
    [SerializeField] private GameObject[]       uiObjects;
    [SerializeField] private float              loadDelay;
    [SerializeField] private TransitionSettings transition;

    private void Start()
    {
        startButton.onClick.AddListener(() => OnStart());

        creditButton.onClick.AddListener(() => OnCredit());
        creditHomeButton.onClick.AddListener(() => OnHome());
    }

    private void OnStart()
    {
        TransitionManager.Instance().Transition("Stage01", transition, loadDelay);
    }

    private void OnCredit()
    {
        TransitionManager.Instance().Transition(transition, 0);

        SetActiveObjects(false);
        creditBackground.SetActive(true);
    }

    private void OnHome()
    {
        TransitionManager.Instance().Transition("Main", transition, loadDelay);
        SetActiveObjects(true);
    }

    private void SetActiveObjects(bool active)
    {
        for(int i = 0; i < uiObjects.Length; i++)
        {
            uiObjects[i].SetActive(active);
        }
    }
}

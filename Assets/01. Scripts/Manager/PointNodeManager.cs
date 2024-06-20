using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNodeManager : MonoBehaviour
{
    public static PointNodeManager Instance { get; private set; }

    [Header("[# Enemy Ai Info ]")]
    public Transform[] enemyPointNode;    

    private void Awake()
    {
        Instance = this;
    }
}

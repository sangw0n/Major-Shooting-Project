// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class PointNodeManager : MonoBehaviour
{
    public static PointNodeManager Instance { get; private set; }

    public Dictionary<int, bool> PointNode = new Dictionary<int, bool>();

    private void Awake()
    {
        Instance = this;
    }
}

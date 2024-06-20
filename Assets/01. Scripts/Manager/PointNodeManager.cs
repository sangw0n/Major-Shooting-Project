using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNodeManager : MonoBehaviour
{
    public static PointNodeManager Instance { get; private set; }

    public Dictionary<Transform, bool>  pointNodeData;
    public Transform[]                  enemyPointNode;    

    private void Awake()
    {
        Instance = this;

        // # Dictionary Init
        pointNodeData = new Dictionary<Transform, bool>();

        for(int i = 0; i < enemyPointNode.Length; i++)
            pointNodeData.Add(enemyPointNode[i], false);
    }

    public void SetPointNodeData(Transform key, bool value)
    {
        pointNodeData[key] = value;
    }

    public bool GetPointNodeDataKey(Transform key)
    {
        return pointNodeData[key];
    }
}

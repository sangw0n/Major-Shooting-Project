using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 다중 오브젝트 풀링
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }
}

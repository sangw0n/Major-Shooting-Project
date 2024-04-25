// # System
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity 
using UnityEngine;
using UnityEngine.Pool;

// 오브젝트 풀의 정보를 저장할 클래스 
[System.Serializable]
public class ObjectPoolData
{
    public string keyName; // 키 이름 
    public GameObject prefab; // 오브젝트 프리팹
    public int initObejctCount; // 오브젝트 초기 생성 개수
}

// 다중 오브젝트 풀링을 관리하는 클래스 
public class PoolManager : MonoBehaviour
{
    // PoolManager 클래스의 싱글톤 인스턴스
    public static PoolManager Instance { get; private set; }

    // 각 오브젝트 풀의 "키 이름", "프리팹", "오브젝트 생성 개수"를 관리하는 배열 변수 
    [SerializeField] private ObjectPoolData[] objectPoolDatas;

    // 생성된 오브젝트 풀들을 저장할 딕셔너리 
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if(Instance == null) Instance = this;   
    }
}

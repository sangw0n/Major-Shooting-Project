// # System
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
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Initialized();

        foreach(KeyValuePair<string, Queue<GameObject>> pool in objectPools)
        {
            Debug.Log($"{pool.Key} : {pool.Value.Count}");
        }
    }

    // ObjectPools 초기화하는 함수 
    private void Initialized()
    {
        // 데이터 배열의 길이를 확인하기 
        int length = objectPoolDatas.Length;
        // 데이터가 없으면 초기화를 종료하기
        if (length == 0) return;

        // 1. 각 오브젝트 풀에 대한 정보를 담을 딕셔너리 생성 
        objectPools = new Dictionary<string, Queue<GameObject>>(length);

        // 2. 모든 오브젝트 풀 데이터에 접근하기 위한 반복문 
        for (int idx = 0; idx < length; idx++)
        {
            // 생성할 오브젝트를 넣어줄 임시 Queue를 생성
            Queue<GameObject> objQueue = new Queue<GameObject>();

            // 오브젝트의 부모가 될 오브젝트 생성 
            GameObject poolParentObj = new GameObject($"{objectPoolDatas[idx].keyName} Parent Obj");
            poolParentObj.transform.SetParent(this.transform);

            // 지정된 개수만큼 오브젝트를 생성하기 위한 반복문 
            for (int idxx = 0; idxx < objectPoolDatas[idx].initObejctCount; idxx++)
            {
                // 생성된 오브젝트를  Queue 에 넣기
                objQueue.Enqueue(CreateObject(idx, poolParentObj.transform));
            }

            // 3. obejctPoolsData[idx] 의 키 이름과 오브젝트를 넣은 Queue를 딕셔너리에 추가
            objectPools.Add(objectPoolDatas[idx].keyName, objQueue);
        }
    }

    // 지정된 인덱스의 프리팹을 생성하는 함수 
    private GameObject CreateObject(int idx, Transform parent)
    {
        // 오브젝트 생성 
        GameObject obj = Instantiate(objectPoolDatas[idx].prefab);

        // 생성된 오브젝트를 parent 의 자식 오브젝트로 설정
        obj.transform.SetParent(parent);

        // 생성한 오브젝트 비활성화
        obj.SetActive(false);

        // 오브젝트 반환
        return obj;
    }

    // 사용하지 않는 중인  오브젝트 가져오는 함수
    public GameObject GetObject(string objName)
    {
        GameObject obj = null;

        // 가져올 objName 의 key 가 존재하지 않다면 함수 종료
        if(objectPools.ContainsKey(objName)) return null;
        
        // 오브젝트 반환 
        return obj;
    }

    // 사용이 끝난 반환하는 함수
}
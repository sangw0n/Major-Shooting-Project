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
    [Header("[# Pool Key and Index]")]
    public string keyName; // 키 이름 
    public int index; // 리스트 내에서의 객체 순서를 나타내는 인덱스

    [Header("[# Obejct Pool Settings]")]
    public Transform parentTransform; // 부모 오브젝트 
    public GameObject objectPrefab; // 오브젝트 프리팹
    public int initObejctCount; // 오브젝트 초기 생성 개수
}

// 다중 오브젝트 풀링을 관리하는 클래스 
public class PoolManager : MonoBehaviour
{
    // PoolManager 의 싱글톤 인스턴스
    public static PoolManager Instance { get; private set; }

    // 오브젝트 풀 설정 정보를 저장하는 리스트
    [SerializeField] private List<ObjectPoolData> objectPoolSettings;

    // 키 이름에 따라 생성된 오브젝트 풀을 관리하는 딕셔너리
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Initialized();
    }

    /// <summary>
    /// ObjectPools를 초기화하는 함수입니다.
    /// </summary>
    private void Initialized()
    {
        // 데이터 배열의 길이를 확인하기 
        int length = objectPoolSettings.Count;
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
            GameObject poolParentObj = new GameObject($"{objectPoolSettings[idx].keyName} Parent Obj");
            objectPoolSettings[idx].parentTransform = poolParentObj.transform;
            poolParentObj.transform.SetParent(this.transform);

            // 지정된 개수만큼 오브젝트를 생성하기 위한 반복문 
            for (int idxx = 0; idxx < objectPoolSettings[idx].initObejctCount; idxx++)
            {
                // 생성된 오브젝트를  Queue 에 넣기
                objQueue.Enqueue(CreateObject(idx, poolParentObj.transform));
            }

            // 3. obejctPoolsData[idx] 의 키 이름과 오브젝트를 넣은 Queue를 딕셔너리에 추가
            objectPools.Add(objectPoolSettings[idx].keyName, objQueue);
        }
    }

    /// <summary>
    /// 지정된 인덱스의 프리팹을 생성하는 함수입니다.
    /// </summary>
    /// <param name="idx">생성할 오브젝트 인덱스</param>
    /// <param name="parent">생성된 오브젝트의 부모</param>
    /// <returns>생성된 오브젝트</returns>
    private GameObject CreateObject(int idx, Transform parent)
    {
        // 오브젝트 생성 
        GameObject obj = Instantiate(objectPoolSettings[idx].objectPrefab);

        // 생성된 오브젝트를 parent 의 자식 오브젝트로 설정
        obj.transform.SetParent(parent);

        // 생성한 오브젝트 비활성화
        obj.SetActive(false);

        // 오브젝트 반환
        return obj;
    }

    /// <summary>
    /// 사용하지 않는 오브젝트를 가져오는 함수입니다.
    /// </summary>
    /// <param name="keyName">가져올 오브젝트의 풀의 키 이름</param>
    /// <returns>사용할 수 있는 오브젝트</returns>
    public GameObject GetObject(string keyName)
    {
        // 가져올 오브젝트의 keyName이 존재하지 않다면 함수 종료
        if (!objectPools.ContainsKey(keyName)) return null;

        // 반환할 오브젝트 
        GameObject obj = null;

        // keyName의 풀에 사용하지 않는 오브젝트가 없다면 
        if (objectPools[keyName].Count == 0)
        {
            Debug.Log($"[PoolManager] {keyName}의 오브젝트 새로 생성");

            // 새로 생성하고 obj 에 가져오기
            int idx = objectPoolSettings.Find(x => x.keyName == keyName).index;
            obj = CreateObject(idx, null);
        }
        // 사용하지 않는 오브젝트가 있다면
        else
            obj = objectPools[keyName].Dequeue();

        // 오브젝트를 활성화 상태로 바꿔주기
        obj.SetActive(true);
        // 오브젝트를 자식에서 해제하기 
        obj.transform.SetParent(null);

        // 오브젝트 반환 
        return obj;
    }

    /// <summary>
    /// 지정된 키 이름에 해당하는 오브젝트 풀로 GameObject를 반환하는 함수입니다.
    /// </summary>
    /// <param name="obj">반환될 오브젝트</param>
    /// <param name="keyName">오브젝트 풀을 식별하는 키 이름</param>
    public void ReturnObject(GameObject obj, string keyName)
    {
        // 오브젝트 비활성화
        obj.SetActive(false);
        // 오브젝트를 해당 keyName의 objectPoolDatas에 있는 부모 Transform에 자식 오브젝트로 넣어줌
        int idx = objectPoolSettings.Find(x => x.keyName == keyName).index;
        obj.transform.SetParent(objectPoolSettings[idx].parentTransform);
        // keyName에 해당하는 오브젝트 풀에 저장 
        objectPools[keyName].Enqueue(obj);
    }

    /// <summary>
    /// 지정된 키 이름에 해당하는 오브젝트 풀로 GameObject를 반환하는 함수입니다.
    /// </summary>
    /// <param name="obj">반환될 오브젝트</param>
    /// <param name="keyName">오브젝트 풀을 식별하는 키 이름</param>
    /// <param name="destoryTime">오브젝트를 반환하기 전에 대기할 시간(초)</param>
    public void ReturnObject(GameObject obj, string keyName, float destoryTime)
    {
        StartCoroutine(CoReturnObject(obj, keyName, destoryTime));
    }

    private IEnumerator CoReturnObject(GameObject obj, string keyName, float destoryTime)
    {
        // 일정 시간동안 대기 
        yield return new WaitForSeconds(destoryTime);

        // 오브젝트 비활성화
        obj.SetActive(false);

        // 자식 오브젝트로 넣어주기
        int idx = objectPoolSettings.Find(x => x.keyName == keyName).index;
        obj.transform.SetParent(objectPoolSettings[idx].parentTransform);

        // keyName 의 풀에 저장 
        objectPools[keyName].Enqueue(obj);
    }
}
// # Unity
using System.Collections;
using UnityEngine;

#region Monster Pattern Manager Class
// 웨이브에서 등장하는 단일 몬스터의 정보 클래스
[System.Serializable]
public class Monster
{
    public GameObject enemy;
    public ObjecyKeyType monsterKeyType;
    public Transform spawnPoint;
}

// 웨이브에 등장할 모든 몬스터의 정보를 담는 클래스
// 이 클래스를 통해 웨이브별 몬스터 배치를 관리
[System.Serializable]
public class Wave
{
    public string waveName;
    // 웨이브에 등장할 몬스터를 관리해줄 배열
    public Monster[] monsters;
}

// 게임 내 하나의 스테이지를 구성하는 클래스 
// 여러 개의 웨이브로 이루어진 스테이지 정보를 관리 
[System.Serializable]
public class Stage
{
    public string stageName;
    public Wave[] waves;
}
#endregion

public class MonsterPatternManager : MonoBehaviour
{
    public static MonsterPatternManager Instance { get; private set; }

    [Header("[# Pattern Data Header]")]
    public Stage[] stages;

    // 현재 스테이지와 웨이브 인덱스
    private int currentStageIndex;
    private int currentWaveIndex;

    // 필드에 남아있는 몬스터 수 
    [SerializeField] private int remainingMonsters;
    public int RemainingMosters { get => remainingMonsters; set => remainingMonsters = value; }

    [SerializeField] private Transform enemyParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Wave());
    }

    private IEnumerator Wave()
    {
        // 초기 몬스터 세팅 
        MonsterInit();

        while (true)
        {
            if (remainingMonsters == 1)
            {
                yield return new WaitForSeconds(0.75f);
                
                // 현재 웨이브 인덱스가 마지막 인덱스가 아닌경우 
                if(currentWaveIndex < stages[currentStageIndex].waves.Length - 1) 
                    currentWaveIndex++; // 다음 웨이브 시작

                // 새로운 몬스터 세팅 
                MonsterInit();
            }

            yield return null;
        }
    }

    [ContextMenu("Monster Init Function")]
    private void MonsterInit()
    {
        // 현재 Wave 정보를 가져와 currentWave 에 초기화
        Wave currentWave = stages[currentStageIndex].waves[currentWaveIndex];

        // 현재 스테이지의 웨이브에 맞는 몬스터들 배치
        for (int index = 0; index < currentWave.monsters.Length; index++)
        {
            // 현재 웨이브에 등장할 몬스터 
            Monster curWaveMonster = currentWave.monsters[index];
            // curWaveMonster 의 키 값을 가져와 몬스터 소환
            GameObject monsterObj = PoolManager.Instance.GetObject(curWaveMonster.monsterKeyType);

            // 생성한 몬스터의 위치 설정
            monsterObj.transform.position = curWaveMonster.spawnPoint.position;
            // 생성한 몬스터 부모 설정
            monsterObj.transform.SetParent(enemyParent);
        }

        remainingMonsters = enemyParent.childCount;
    }
}

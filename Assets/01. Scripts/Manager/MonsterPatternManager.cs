// # Unity
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
    [Header("[# Pattern Data Header]")]
    public Stage[] stages;

    // 현재 스테이지와 웨이브 인덱스
    private int currentStageIndex;
    private int currentWaveIndex;

    private Wave currentWave;

    private void Start()
    {
        MonsterInit();
    }

    [ContextMenu("Monster Init Function")]
    private void MonsterInit()
    {
        // 현재 Wave 정보를 가져와 currentWave 에 초기화
        currentWave = stages[currentStageIndex].waves[currentWaveIndex];

        // 현재 스테이지의 웨이브에 맞는 몬스터들 배치
        for (int index = 0; index < currentWave.monsters.Length; index++)
        {
            Monster curWaveMonster = currentWave.monsters[index];
            GameObject monsterObj = PoolManager.Instance.GetObject(curWaveMonster.monsterKeyType);

            // 생성한 몬스터의 위치 설정
            monsterObj.transform.position = curWaveMonster.spawnPoint.position;
        }
    }
}

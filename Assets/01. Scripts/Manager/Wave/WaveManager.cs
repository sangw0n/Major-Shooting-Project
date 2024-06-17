using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set;}

    // ----------- [ SerializeField Field ] -----------
    [ElementName("Wave")]
    [SerializeField] private Wave[]          waves;            // 스테이지의 모든 Wave 관리
    [SerializeField] private List<Enemy>     activeEnemys;   // 현재 활성화된 몬스터들을 저장할 List
    
    // ----------- [ Private Field ] -----------
    private int currentWaveIndex;                               // 현재 웨이브 인덱스
    private int maxWaveIndex;                                   // 최대 웨이브 인덱스

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentWaveIndex = 0;
        maxWaveIndex     = waves.Length - 1;

        activeEnemys     = new List<Enemy>();

        StartWave();
    }

    private void StartWave()
    {
        StartCoroutine(Co_StartWave());
    }

    ///<summary> Wave를 시작하는 함수 </summary>
    private IEnumerator Co_StartWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for(int index = 0; index < currentWave.spawnDatas.Length; index++)
        {
            SpawnData spawnData = currentWave.spawnDatas[index];

            for(int i = 0; i < currentWave.spawnDatas[index].spawnPoint.Length; i++)
            {
                GameObject cloneEnemy = Instantiate(spawnData.enemyPrefab[i], spawnData.spawnPoint[i].position, Quaternion.identity);
                activeEnemys.Add(cloneEnemy.GetComponent<Enemy>());
            }

            if(spawnData.spawnTime != 0.0f)
            {
                yield return new WaitForSeconds(spawnData.spawnTime);
            }
        }
    }


    ///<summary> 적이 죽었을 때 ActiveEnemy List에서 삭제시키는 함수</summary>
    public void RemoveActiveEnemy(Enemy enemy)
    {
        activeEnemys.Remove(enemy);
    }
    

    ///<summary> 남은 몬스터 수를 체크해 기준에 맞다면 다음 웨이브 시작시키는 함수</summary>
    public void CheckRemainingMonstersForNextWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        if(currentWave.nextWaveThreshold >= activeEnemys.Count)
        {
            currentWaveIndex++;

            if(currentWaveIndex > maxWaveIndex)
            {
                return;
            }

            StartWave();
        }
    }
}

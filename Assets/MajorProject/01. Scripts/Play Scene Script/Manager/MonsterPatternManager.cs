namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;
    using UnityEngine.SceneManagement;

    #region Game Pattern Class
    /// <summary> 몬스터 오브젝트와 스폰 위치를 정의할 클래스 </summary>
    [System.Serializable]
    public class MonsterSpawnInfo
    {
        public ObjecyKeyType objecyKey;
        public Transform spawnPoint;
    }

    /// <summary> 웨이브에 등장할 몬스터들의 정보를 정의할 클래스 </summary> 
    [System.Serializable]
    public class Wave
    {
        [ElementName("Monster")]
        public MonsterSpawnInfo[] monsterSpawnInfos;
        public int nextWaveRequireMonsterCount;
    }
    #endregion

    public class MonsterPatternManager : MonoBehaviour
    {
        public static MonsterPatternManager Instance { get; private set; }

        [ElementName("Wave")]
        [SerializeField] private Wave[] waves;
        [SerializeField] private Transform monsterParent;

        private Wave currentWave;

        private int remainingMonsterCount; // 필드에 남아있는 몬스터 수
        private int currentWaveIndex = 0; // 현재 웨이브 인덱스 

        // Property ( 프로퍼티 )
        public int CurrentWaveIndex
        {
            get { return currentWaveIndex; }
            set
            {
                currentWaveIndex = value;

                // 다음 웨이브가 없다면 스테이지 클리어
                if (currentWaveIndex >= waves.Length)
                {
                    SceneManager.LoadScene("99. Game Clear");
                    return;
                }
                currentWave = waves[currentWaveIndex];
            }
        }
        public int RemainingMonsterCount
        {
            get { return remainingMonsterCount; }
            set { remainingMonsterCount = value; }
        }

        private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        private void Awake()
        {
            Instance = this;

            // 변수 초기화
            currentWave = waves[currentWaveIndex];
            remainingMonsterCount = currentWave.monsterSpawnInfos.Length;

            // 함수 실행
            SpawnMonster();
            StartCoroutine(OnCheckMonsterTick());
        }

        private IEnumerator OnCheckMonsterTick()
        {
            while (true)
            {
                // 만약 필드에 남아있어야 할 몬스터의 기준 수 보다 낮으면 다음 웨이브 
                if (remainingMonsterCount <= currentWave.nextWaveRequireMonsterCount)
                {
                    // 다음 웨이브 시작
                    CurrentWaveIndex++;

                    // 웨이브를 전부 클리어하면 반복문
                    if (currentWaveIndex >= waves.Length)
                        break;

                    // 몬스터 소환
                    SpawnMonster();

                    // 필드에 남아있는 몬스터 수 체크 
                    remainingMonsterCount = monsterParent.childCount;
                }

                yield return waitForSeconds;
            }
        }

        private void SpawnMonster()
        {
            for (int index = 0; index < currentWave.monsterSpawnInfos.Length; index++)
            {
                // 몬스터 소환 
                GameObject monster = PoolManager.Instance.GetObject(currentWave.monsterSpawnInfos[index].objecyKey);

                // 몬스터 부모 오브젝트 설정
                monster.transform.SetParent(monsterParent);

                // 몬스터 위치 배치
                monster.transform.position = currentWave.monsterSpawnInfos[index].spawnPoint.position;
            }
        }
    }
}
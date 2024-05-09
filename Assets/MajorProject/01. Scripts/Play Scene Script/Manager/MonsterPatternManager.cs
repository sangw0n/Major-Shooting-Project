namespace MajorProject.Play
{
    // # System
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor.SceneManagement;

    // # Unity
    using UnityEngine;

    /// <summary> 몬스터 오브젝트와 스폰 위치를 정의할 클래스 </summary>
    [System.Serializable]
    public class MonsterSpawnInfo
    {
        public GameObject monsterObject;
        public Transform spawnPoint;
    }    

    /// <summary> 웨이브에 등장할 몬스터들의 정보를 정의할 클래스 </summary> 
    [System.Serializable]
    public class Wave
    {
        public MonsterSpawnInfo[] monsterSpawnInfos;
    }

    /// <summary> Wave 를 관리해줄 클래스 </summary>
    [System.Serializable]
    public class Stage
    {
        public Wave[] waves;
    }

    public class MonsterPatternManager : MonoBehaviour
    {
        [SerializeField] private Stage[] stages;
    }
}
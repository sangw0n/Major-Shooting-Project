using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public GameObject[] enemyPrefab;
    public Transform[]  spawnPoint;
    public float      spawnTime;
}
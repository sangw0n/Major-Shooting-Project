using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    [ElementName("SpawnData")]
    public SpawnData[] spawnDatas;
    public int         nextWaveThreshold;
}

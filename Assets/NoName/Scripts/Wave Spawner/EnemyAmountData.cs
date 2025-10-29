using System;
using UnityEngine;

[Serializable]
public class EnemyAmountData
{
    public GameObject prefab;
    public int amount;

    public float minSpawnDelay = 0.3f;
    public float maxSpawnDelay = 1.5f;
}

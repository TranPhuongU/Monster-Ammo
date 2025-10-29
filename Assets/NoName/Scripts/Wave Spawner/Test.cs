using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private Transform spawnPos;

    private void Start()
    {
        StartCoroutine(SpawnAllWave());
    }

    IEnumerator SpawnAllWave()
    {
        foreach(var wave in levelData[0].waves)
        {
            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        List<GameObject> spawnList = new List<GameObject>();

        foreach(var enemy in wave.enemies)
        {
            for(int i = 0; i < enemy.amount; i++)
            {
                spawnList.Add(enemy.prefab);
            }
        }

        foreach(var prefab in spawnList)
        {
            Instantiate(prefab, spawnPos.position, Quaternion.identity);

            var enemyData = wave.enemies.Find(e => e.prefab == prefab);

            if(enemyData != null)
            {
                float delay = Random.Range(enemyData.minSpawnDelay, enemyData.maxSpawnDelay);

                yield return new WaitForSeconds(delay);
            }
        }
    }
}

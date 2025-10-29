using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private LevelData[] levelData;            
    [SerializeField] private Transform spawnPoint;
    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        foreach (var wave in levelData[0].waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            StartCoroutine(UIManager.instance.ShowNextWaveText());
            yield return new WaitForSeconds(3.5f); // đợi 1 giây giữa các wave
        }

        Debug.Log("Level Complete!");
        // TODO: thông báo qua màn, gọi LevelManager
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        GameManager.instance.SetGameState(GameManager.GameState.LevelComplete);
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        List<(GameObject prefab, float minDelay, float maxDelay)> spawnList = new();

        foreach (var enemy in wave.enemies)
        {
            for (int i = 0; i < enemy.amount; i++)
            {
                spawnList.Add((enemy.prefab, enemy.minSpawnDelay, enemy.maxSpawnDelay));
            }
        }

        Shuffle(spawnList); // trộn enemy lẫn với nhau nhưng giờ chưa cần tới, xóa đi cũng được

        foreach (var (prefab, minDelay, maxDelay) in spawnList)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            float delay = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }


    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

}
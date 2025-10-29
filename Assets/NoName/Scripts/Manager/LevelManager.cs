using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private Transform spawnPoint;
    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }


    IEnumerator SpawnAllWaves()
    {
        int currentLevel = GetLevel();
        currentLevel = currentLevel % levelData.Length;

        var waves = levelData[currentLevel].waves;
        for (int i = 0; i < waves.Count; i++)
        {
            yield return StartCoroutine(SpawnWave(waves[i]));

            // Chỉ hiện "Next Wave" nếu chưa phải wave cuối
            if (i < waves.Count - 1)
            {
                StartCoroutine(UIManager.instance.ShowNextWaveText());
                yield return new WaitForSeconds(3.5f);
            }
        }

        // Chờ cho đến khi không còn enemy nào trên scene
        while (GameObject.FindObjectsOfType<EnemyController>().Length > 0)
        {
            yield return null;
        }

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

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("level", 0);
    }

}
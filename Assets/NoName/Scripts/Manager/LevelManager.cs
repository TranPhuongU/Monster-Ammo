using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private GameObject[] levelPrefabs;

    [SerializeField] private Slider spawnProgressBar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI levelText;

    public static Action musicStop;
    private void Start()
    {

        GameManager.onGameStateChanged += OnGameStateChanged;

        int currentLevel = GetLevel();
        currentLevel = currentLevel % levelData.Length;

    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= OnGameStateChanged;
    }


    private void LevelSpawner()
    {
        int currentLevel = GetLevel();
        currentLevel = currentLevel % levelData.Length;

        Instantiate(levelPrefabs[currentLevel], Vector3.zero, Quaternion.identity);

    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Game)
        {
            // Khi người chơi ấn Play
            LevelSpawner();
            StartCoroutine(SpawnAllWaves());
        }
    }


    IEnumerator SpawnAllWaves()
    {

        int currentLevel = GetLevel();
        currentLevel = currentLevel % levelData.Length;

        var waves = levelData[currentLevel].waves;


        for (int i = 0; i < waves.Count; i++)
        {
            levelText.text = $"Wave {i + 1}/{waves.Count}";


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

        musicStop?.Invoke();

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

        // Nếu bạn không cần trộn, có thể xóa
        Shuffle(spawnList);

        // Đặt thanh tiến trình về 0 trước khi spawn
        spawnProgressBar.gameObject.SetActive(true);
        spawnProgressBar.value = 0f;

        int totalEnemies = spawnList.Count;
        int spawnedCount = 0;

        foreach (var (prefab, minDelay, maxDelay) in spawnList)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            spawnedCount++;

            // Cập nhật tiến trình
            spawnProgressBar.value = (float)spawnedCount / totalEnemies;

            float delay = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }

        // Khi spawn xong toàn bộ enemy
        spawnProgressBar.value = 1f;
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
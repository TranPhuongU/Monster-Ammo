using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject nextWave;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject levelCompletePanel;
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        gamePanel.SetActive(true);
        gameoverPanel.SetActive(false);
        //settingPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        GameManager.onGameStateChanged += GameStateChangedCallBack;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallBack;
    }

    private void GameStateChangedCallBack(GameManager.GameState _gameState)
    {
        if(_gameState == GameManager.GameState.Gameover)
            ShowGameover();
        else if(_gameState == GameManager.GameState.LevelComplete)
            ShowLevelComplete();
    }
    public IEnumerator ShowNextWaveText()
    {
        nextWave.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        nextWave.SetActive(false);
    }

    public void ShowGameover()
    {
        gameoverPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void RetryButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayButtonPressed()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Game);
        gamePanel.SetActive(true);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GunData[] allGuns; // Thêm dòng này

    public enum GameState
    {
        Menu,
        Game,
        LevelComplete,
        Gameover
    }
    private GameState gameState;
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        SetGameState(GameState.Menu);

    }

    public void SetGameState(GameState _gameState)
    {
        this.gameState = _gameState;
        onGameStateChanged?.Invoke(_gameState);
    }

    public GameState GetCurrentState()
    {
        return gameState;
    }
}

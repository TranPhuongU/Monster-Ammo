using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    [Header("Sounds")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource humanShootSound;
    [SerializeField] private AudioSource canonShootSound;
    [SerializeField] private AudioSource levelComplateSound;
    [SerializeField] private AudioSource gameoverSound;
    [SerializeField] private AudioSource musicSound;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    void Start()
    {

        GameManager.onGameStateChanged += GameStateChangedCallback;
        CanonController.onShoot += PlayCanonShoot;
        Human.onShoot += HumanShootSound;
        LevelManager.musicStop += StopMusicSound;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        CanonController.onShoot -= PlayCanonShoot;
        Human.onShoot -= HumanShootSound;
        LevelManager.musicStop -= StopMusicSound;
    }
    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.LevelComplete)
        {
            levelComplateSound.Play();

            DisableSounds();
        }
        else if (gameState == GameManager.GameState.Gameover)
        {
            gameoverSound.Play();

            DisableSounds();
        }
        else if (gameState == GameManager.GameState.Game)
        {
            EnableSounds();

            musicSound.Play();
        }
    }

    private void HumanShootSound()
    {
        humanShootSound.Play();
    }
    private void StopMusicSound()
    {
        musicSound.Stop();
    }

    private void PlayCanonShoot()
    {
        canonShootSound.pitch = 5f;

        canonShootSound.Play();
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void DisableSounds()
    {
        humanShootSound.volume = 0;
        canonShootSound.volume = 0;
        //levelComplateSound.volume = 0;
        //gameoverSound.volume = 0;
        //buttonSound.volume = 0;
        musicSound.volume = 0;
    }

    public void EnableSounds()
    {
        humanShootSound.volume = 1;
        canonShootSound.volume = 1;
        //levelComplateSound.volume = 1;
       // gameoverSound.volume = 1;
        //buttonSound.volume = 1;
        musicSound.volume = 1;
    }
}

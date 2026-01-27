using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject nextWave;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject mainMenu;

    [Header("Info Panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI gunShootingSpeedText;
    [SerializeField] private TextMeshProUGUI gunBulletsPerShootText;
    [SerializeField] private TextMeshProUGUI gunDamageText;
    [SerializeField] private Image gunIcon;

    [Header("Setting")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        shopPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        GameManager.onGameStateChanged += GameStateChangedCallBack;

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
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

    public void ShowSetting()
    {
        settingPanel.SetActive(true);
        gamePanel.SetActive(false);
        mainMenu.SetActive(false);

        SoundsManager.instance.DisableSounds();
        Time.timeScale = 0f; // Dừng game

    }
    public void HideSetting()
    {
        settingPanel.SetActive(false);

        // Nếu đang ở màn game
        if (GameManager.instance.GetCurrentState() == GameManager.GameState.Game)
        {
            gamePanel.SetActive(true);
        }
        // Nếu đang ở menu
        else if (GameManager.instance.GetCurrentState() == GameManager.GameState.Menu)
        {
            mainMenu.SetActive(true);
        }

        SoundsManager.instance.EnableSounds();
        Time.timeScale = 1f; // tiep tuc
    }
    public void ShowPause()
    {
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);

        SoundsManager.instance.DisableSounds();
        Time.timeScale = 0f; // Dừng game

    }

    public void HidePause()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);

        SoundsManager.instance.EnableSounds();
        Time.timeScale = 1f; // tiep tuc

    }

    public void HomeButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void RetryButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayButtonPressed()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Game);
        gamePanel.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowShopButton()
    {
        mainMenu.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void BackButton()
    {
        shopPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();

    }

    public void ShowGunInfo(int index)
    {
        GunData gun = GameManager.instance.allGuns[index];
        bool unlocked = ShopManager.instance.gunButtons[index].IsUnlocked();

        infoPanel.SetActive(true);

        if (unlocked)
        {
            gunShootingSpeedText.text = "Cooldown: " + gun.cooldown.ToString();
            gunBulletsPerShootText.text = "Bullets Per Shoot: " + gun.bulletsPerShot.ToString();
            gunDamageText.text = "Damage: " + gun.damage.ToString(); 
            gunIcon.sprite = gun.gunSprite;
            gunIcon.color = Color.white;
        }
        else
        {
            gunShootingSpeedText.text = "Cooldown: ???";
            gunBulletsPerShootText.text = "Bullets Per Shoot: ???";
            gunDamageText.text = "Damage: ???";
            gunIcon.sprite = gun.gunSprite;
            gunIcon.color = Color.black;
        }
    }

    public void HideGunInfo()
    {
        infoPanel.SetActive(false);
    }

}

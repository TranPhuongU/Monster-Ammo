using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        shopPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            PlayerPrefs.DeleteAll();
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
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
}

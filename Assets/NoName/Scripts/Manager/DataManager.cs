using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("Coin Texts")]
    [SerializeField] private Text[] coinsTexts;
    private int coins;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        coins = PlayerPrefs.GetInt("coins", 100);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateCoinsTexts()
    {
        foreach (Text cointText in coinsTexts)
        {
            cointText.text = coins.ToString();
        }
    }

    public void AddCoins(int _amount)
    {
        coins += _amount;
        UpdateCoinsTexts();

        PlayerPrefs.SetInt("coins", coins);
    }

    public int GetCoins()
    {
        return coins;
    }

    public void UseCoins(int amount)
    {
        coins -= amount;

        UpdateCoinsTexts();

        PlayerPrefs.SetInt("coins", coins);
    }
}

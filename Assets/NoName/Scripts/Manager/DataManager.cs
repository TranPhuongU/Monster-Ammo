using System.Collections;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("Coin Texts")]
    [SerializeField] private TextMeshProUGUI[] moneysText;

    private int money;
    private Coroutine animateCoroutine;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        money = PlayerPrefs.GetInt("coins", 1000);
    }

    void Start()
    {
        UpdateCoinsTextsInstant();
    }

    private void UpdateCoinsTextsInstant()
    {
        foreach (TextMeshProUGUI moneyText in moneysText)
            moneyText.text = money.ToString();
    }

    public void AddCoins(int _amount)
    {
        int oldMoney = money;
        money += _amount;
        PlayerPrefs.SetInt("coins", money);

        if (animateCoroutine != null)
            StopCoroutine(animateCoroutine);

        animateCoroutine = StartCoroutine(AnimateMoneyText(oldMoney, money));
    }

    public void UseCoins(int amount)
    {
        int oldMoney = money;
        money -= amount;
        PlayerPrefs.SetInt("coins", money);

        if (animateCoroutine != null)
            StopCoroutine(animateCoroutine);

        animateCoroutine = StartCoroutine(AnimateMoneyText(oldMoney, money));
    }

    private IEnumerator AnimateMoneyText(int startValue, int endValue)
    {
        float duration = 0.5f; // thời gian chạy hiệu ứng
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentValue = (int)Mathf.Lerp(startValue, endValue, t);

            foreach (TextMeshProUGUI moneyText in moneysText)
                moneyText.text = currentValue.ToString();

            yield return null;
        }

        foreach (TextMeshProUGUI moneyText in moneysText)
            moneyText.text = endValue.ToString();
    }

    public int GetCoins()
    {
        return money;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

   

    [Header("Elements")]
    [SerializeField] private Button buyButton;
    public GunButton[] gunButtons;

    [Header("Skins")]
    [SerializeField] private Sprite[] skins;

    [Header("Pricing")]
    [SerializeField] private int skinPrice;
    [SerializeField] private Text priceText;

    public static Action<int> onSkinSelected;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        UnlockSkin(0);

        priceText.text = skinPrice.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        ConfigureButtons();

        UpdateBuyButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UnlockSkin(Random.Range(0, gunButtons.Length));

        if (Input.GetKeyDown(KeyCode.D))
            PlayerPrefs.DeleteAll();
    }

    private void ConfigureButtons()
    {
        for (int i = 0; i < gunButtons.Length; i++)
        {
            bool unlocked = PlayerPrefs.GetInt("skinButton" + i) == 1;

            gunButtons[i].Configure(skins[i], unlocked);

            // can dong nay de lưu lại biến i. Vì khi chạy SelectSkin sẽ không được gọi luôn nên biến i đã thay đổi thành 9
            int skinIndex = i;

            gunButtons[i].GetButton().onClick.AddListener(() => SelectSkin(skinIndex));

            gunButtons[i].gunIndex = i;

        }
    }

    public void UnlockSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("skinButton" + skinIndex, 1);
        gunButtons[skinIndex].Unlock();
    }

    private void UnlockSkin(GunButton skinButton)
    {
        int skinIndex = skinButton.transform.GetSiblingIndex();
        UnlockSkin(skinIndex);
    }

    private void SelectSkin(int skinIndex)
    {
        for (int i = 0; i < gunButtons.Length; i++)
        {
            if (skinIndex == i)
                gunButtons[i].Select();
            else
                gunButtons[i].Deselect();
        }

        onSkinSelected?.Invoke(skinIndex);

        PlayerPrefs.SetInt("SelectedGunIndex", skinIndex); // Lưu lựa chọn vào PlayerPrefs
        PlayerPrefs.Save(); // Đảm bảo lưu ngay


    }

    public void BuySkin()
    {
        List<GunButton> skinButtonList = new List<GunButton>();

        for (int i = 0; i < gunButtons.Length; i++)
        {
            if (!gunButtons[i].IsUnlocked())
                skinButtonList.Add(gunButtons[i]);
        }

        if (skinButtonList.Count <= 0)
            return;

        GunButton randomSkinButton = skinButtonList[Random.Range(0, skinButtonList.Count)];

        UnlockSkin(randomSkinButton);
        SelectSkin(randomSkinButton.transform.GetSiblingIndex());

        DataManager.instance.UseCoins(skinPrice);

        UpdateBuyButton();
    }

    public void UpdateBuyButton()
    {
        if (DataManager.instance.GetCoins() < skinPrice)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;
    }

   

}

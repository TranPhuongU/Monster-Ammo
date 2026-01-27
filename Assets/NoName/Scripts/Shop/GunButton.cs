using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public int gunIndex;


    [Header("Elements")]
    [SerializeField] private Button thisButton;
    [SerializeField] private Image skinImage;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject selector;

    private bool unlocked;

    public void Configure(Sprite skinSprite, bool unlocked)
    {
        skinImage.sprite = skinSprite;
        this.unlocked = unlocked;

        if (unlocked)
            Unlock();
        else
            Lock();

    }

    public void Unlock()
    {
        thisButton.interactable = true;
        skinImage.gameObject.SetActive(true);
        lockImage.SetActive(false);

        unlocked = true;

    }
    public void Lock()
    {
        thisButton.interactable = false;
        skinImage.gameObject.SetActive(false);
        lockImage.SetActive(true);
    }

    public void Select()
    {
        selector.SetActive(true);
    }

    public void Deselect()
    {
        selector.SetActive(false);
    }

    public bool IsUnlocked()
    {
        return unlocked;
    }

    public Button GetButton()
    {
        return thisButton;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.instance.ShowGunInfo(gunIndex);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIManager.instance.HideGunInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.ShowGunInfo(gunIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideGunInfo();
    }
}

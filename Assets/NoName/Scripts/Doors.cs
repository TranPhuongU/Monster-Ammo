using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BonusType { Addition, Difference, Product }

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private TextMeshPro doorText;
    [SerializeField] private Collider2D colliderDoor;

    [Header("Settings")]
    [SerializeField] private BonusType doorBonusType;
    public int doorBonusAmount;

    [SerializeField] private Color bonusColor;
    [SerializeField] private Color penaltyColor;

    public static Action updateDoorText;

    private void Awake()
    {
        updateDoorText += UpdateDoorText;
    }

    private void OnDestroy()
    {
        updateDoorText -= UpdateDoorText;
    }
    private void Start()
    {
        ConfigureDoors();
    }

    private void ConfigureDoors()
    {
        switch (doorBonusType)
        {
            case BonusType.Addition:
                doorRenderer.color = bonusColor;
                doorText.text = "+" + doorBonusAmount;
                break;

            case BonusType.Difference:
                doorRenderer.color = penaltyColor;
                doorText.text = "-" + doorBonusAmount;
                break;

            case BonusType.Product:
                doorRenderer.color = bonusColor;
                doorText.text = "x" + doorBonusAmount;
                break;
        }
    }

    public int GetBonusAmount()
    {
        return doorBonusAmount;
    }

    public BonusType GetBonusType()
    {
        return doorBonusType;
    }
    public void UpdateDoorText()
    {
        switch(doorBonusType)
        {
            case BonusType.Addition:
                doorText.text = "+" + doorBonusAmount;
                break;

            case BonusType.Difference:
                doorText.text = "-" + doorBonusAmount;
                break;
            }

        }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource

}

public enum ConsumableType
{
    Health,
    Stamina
}


public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemObject : MonoBehaviour
{
    [Header("info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

}


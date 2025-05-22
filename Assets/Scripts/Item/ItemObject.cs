using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}


// 소모 아이템 사용 시 변경될 Conditions
public enum ConsumableType
{
    Hunger,
    Health
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class itemObject : ScriptableObject
{

    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPerfab;
    public itemObject item;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}
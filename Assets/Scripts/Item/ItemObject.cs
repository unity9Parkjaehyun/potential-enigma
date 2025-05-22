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


// �Ҹ� ������ ��� �� ����� Conditions
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
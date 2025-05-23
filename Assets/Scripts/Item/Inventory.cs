using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �κ��� InventorySlot - Inventory.cs ���� �ȿ� ���� �־�� �մϴ�!
[System.Serializable]
public class InventorySlot
{
    public itemObject item;
    public int quantity;

    public InventorySlot(itemObject _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }

    public void AddQuantity(int value)
    {
        quantity += value;
    }
}

// �̰��� ���� ������Ʈ�� �߰��� Inventory Ŭ����
public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int maxSlots = 20;

    [Header("Current Inventory")]
    public List<InventorySlot> slots = new List<InventorySlot>();  // �� ���⼭ InventorySlot ���!

    [Header("UI")]
    public GameObject inventoryUI;
    public Transform slotsParent;
    public GameObject slotPrefab;

    private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();
    private bool isOpen = false;

    void Start()
    {
        // UI ���� ����
        if (slotsParent != null && slotPrefab != null)
        {
            for (int i = 0; i < maxSlots; i++)
            {
                GameObject slotGO = Instantiate(slotPrefab, slotsParent);
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                if (slotUI != null)
                {
                    slotUI.index = i;
                    slotUI.inventory = this;
                    uiSlots.Add(slotUI);
                }
            }
        }

        if (inventoryUI != null)
            inventoryUI.SetActive(false);

        UpdateUI();
    }

    public bool AddItem(itemObject item, int quantity = 1)
    {
        // ���� ������ �������� ��� ���� ���Կ� �߰�
        if (item.canStack)
        {
            InventorySlot existingSlot = slots.Find(slot => slot.item == item);
            if (existingSlot != null && existingSlot.quantity < item.maxStackAmount)
            {
                int addAmount = Mathf.Min(quantity, item.maxStackAmount - existingSlot.quantity);
                existingSlot.AddQuantity(addAmount);
                UpdateUI();
                return true;
            }
        }

        // �� ���Կ� �߰�
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(item, quantity));
            UpdateUI();
            return true;
        }

        Debug.Log("�κ��丮�� ���� á���ϴ�!");
        return false;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots.RemoveAt(index);
            UpdateUI();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isOpen);

            // �κ��丮�� ������ ���콺 Ŀ�� ǥ��
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;

            // ���� �Ͻ����� (���û���)
            Time.timeScale = isOpen ? 0f : 1f;
        }
    }

    void UpdateUI()
    {
        // ��� UI ���� �ʱ�ȭ
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < slots.Count)
            {
                uiSlots[i].UpdateSlot(slots[i]);
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
    }

    public void UseItem(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            InventorySlot slot = slots[index];

            // ��� �������� ���
            if (slot.item.type == ItemType.Equipable)
            {
                EquipItem(slot.item);
                RemoveItem(index);
            }
            // �Һ� �������� ���
            else if (slot.item.type == ItemType.Consumable)
            {
                // �Һ� ������ ��� ����
                slot.quantity--;
                if (slot.quantity <= 0)
                {
                    RemoveItem(index);
                }
                else
                {
                    UpdateUI();
                }
            }
        }
    }

    void EquipItem(itemObject item)
    {
        // �÷��̾��� ��� �ý��ۿ� ������ ����
        PlayerEquipment equipment = GetComponent<PlayerEquipment>();
        if (equipment != null)
        {
            equipment.EquipWeapon(item);
        }
    }
}
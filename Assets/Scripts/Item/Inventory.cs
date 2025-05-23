using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 부분이 InventorySlot - Inventory.cs 파일 안에 같이 있어야 합니다!
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

// 이것이 실제 컴포넌트로 추가할 Inventory 클래스
public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int maxSlots = 20;

    [Header("Current Inventory")]
    public List<InventorySlot> slots = new List<InventorySlot>();  // ← 여기서 InventorySlot 사용!

    [Header("UI")]
    public GameObject inventoryUI;
    public Transform slotsParent;
    public GameObject slotPrefab;

    private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();
    private bool isOpen = false;

    void Start()
    {
        // UI 슬롯 생성
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
        // 스택 가능한 아이템인 경우 기존 슬롯에 추가
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

        // 새 슬롯에 추가
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(item, quantity));
            UpdateUI();
            return true;
        }

        Debug.Log("인벤토리가 가득 찼습니다!");
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

            // 인벤토리가 열리면 마우스 커서 표시
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;

            // 게임 일시정지 (선택사항)
            Time.timeScale = isOpen ? 0f : 1f;
        }
    }

    void UpdateUI()
    {
        // 모든 UI 슬롯 초기화
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

            // 장비 아이템인 경우
            if (slot.item.type == ItemType.Equipable)
            {
                EquipItem(slot.item);
                RemoveItem(index);
            }
            // 소비 아이템인 경우
            else if (slot.item.type == ItemType.Consumable)
            {
                // 소비 아이템 사용 로직
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
        // 플레이어의 장비 시스템에 아이템 장착
        PlayerEquipment equipment = GetComponent<PlayerEquipment>();
        if (equipment != null)
        {
            equipment.EquipWeapon(item);
        }
    }
}
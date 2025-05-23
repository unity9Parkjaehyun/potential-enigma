using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public GameObject highlightEffect;

    [HideInInspector]
    public int index;
    [HideInInspector]
    public Inventory inventory;

    private InventorySlot currentSlot;

    void Start()
    {
        if (highlightEffect != null)
            highlightEffect.SetActive(false);
    }

    public void UpdateSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (iconImage != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;
        }

        if (quantityText != null)
        {
            if (slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
    }

    public void ClearSlot()
    {
        currentSlot = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSlot != null)
        {
            // 우클릭으로 아이템 사용/장착
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                inventory.UseItem(index);
            }
            // 좌클릭으로 아이템 정보 표시 (선택사항)
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                ShowItemInfo();
            }
        }
    }

    void ShowItemInfo()
    {
        // 아이템 정보 툴팁 표시 (나중에 구현)
        Debug.Log($"아이템: {currentSlot.item.displayName}\n설명: {currentSlot.item.description}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightEffect != null && currentSlot != null)
            highlightEffect.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightEffect != null)
            highlightEffect.SetActive(false);
    }
}
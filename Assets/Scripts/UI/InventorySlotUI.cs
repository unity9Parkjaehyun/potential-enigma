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
            // ��Ŭ������ ������ ���/����
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                inventory.UseItem(index);
            }
            // ��Ŭ������ ������ ���� ǥ�� (���û���)
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                ShowItemInfo();
            }
        }
    }

    void ShowItemInfo()
    {
        // ������ ���� ���� ǥ�� (���߿� ����)
        Debug.Log($"������: {currentSlot.item.displayName}\n����: {currentSlot.item.description}");
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
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public itemObject item;
    public int quantity = 1;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public float rotationSpeed = 50f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;

    private Vector3 startPos;
    private bool canPickup = false;
    private Transform player;

    void Start()
    {
        startPos = transform.position;

        // InteractableObject ������Ʈ�� �ִٸ� ����
        InteractableObject interactable = GetComponent<InteractableObject>();
        if (interactable != null && item != null)
        {
            interactable.objectInfo.objectName = item.displayName;
            interactable.objectInfo.description = "EŰ�� ���� ȹ��";
            interactable.objectInfo.icon = item.icon;
        }
    }

    void Update()
    {
        // ������ ȸ���� ���Ʒ� ������
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // �÷��̾ ������ �ִ��� Ȯ��
        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
        }

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            canPickup = distance <= pickupRange;
        }
    }

    public void TryPickup()
    {
        if (!canPickup) return;

        Inventory playerInventory = player.GetComponent<Inventory>();
        if (playerInventory != null)
        {
            if (playerInventory.AddItem(item, quantity))
            {
                Debug.Log($"{item.displayName} ȹ��!");
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}

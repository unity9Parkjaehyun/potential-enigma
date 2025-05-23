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

        // InteractableObject 컴포넌트가 있다면 설정
        InteractableObject interactable = GetComponent<InteractableObject>();
        if (interactable != null && item != null)
        {
            interactable.objectInfo.objectName = item.displayName;
            interactable.objectInfo.description = "E키를 눌러 획득";
            interactable.objectInfo.icon = item.icon;
        }
    }

    void Update()
    {
        // 아이템 회전과 위아래 움직임
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 플레이어가 가까이 있는지 확인
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
                Debug.Log($"{item.displayName} 획득!");
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

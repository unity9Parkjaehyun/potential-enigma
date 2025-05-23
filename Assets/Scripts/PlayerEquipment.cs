using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Equipment Slots")]
    public Transform weaponHolder; // ���⸦ �� ��ġ (������)

    [Header("Current Equipment")]
    public itemObject currentWeapon;
    private GameObject currentWeaponObject;

    [Header("Combat Settings")]
    public float attackDamage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // ���� Ȧ���� ������ �ڵ����� ã��
        if (weaponHolder == null)
        {
            Transform rightHand = transform.Find("CameraContainer/RightHand");
            if (rightHand == null)
            {
                // ī�޶� �����̳� �Ʒ��� ���� Ȧ�� ����
                GameObject holder = new GameObject("WeaponHolder");
                holder.transform.SetParent(transform.Find("CameraContainer"));
                holder.transform.localPosition = new Vector3(0.5f, -0.3f, 0.7f); // ȭ�� ������ �Ʒ�
                holder.transform.localRotation = Quaternion.identity;
                weaponHolder = holder.transform;
            }
            else
            {
                weaponHolder = rightHand;
            }
        }
    }

    public void EquipWeapon(itemObject weaponItem)
    {
        // ���� ���� ����
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        // �� ���� ����
        currentWeapon = weaponItem;

        if (weaponItem.dropPerfab != null)
        {
            currentWeaponObject = Instantiate(weaponItem.dropPerfab, weaponHolder);
            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeaponObject.transform.localRotation = Quaternion.identity;

            // ������ Collider�� ��Ȱ��ȭ (�տ� ��� ���� ���� �浹 ����)
            Collider weaponCollider = currentWeaponObject.GetComponent<Collider>();
            if (weaponCollider != null)
                weaponCollider.enabled = false;

            // ItemPickup ������Ʈ�� �ִٸ� ����
            ItemPickup pickup = currentWeaponObject.GetComponent<ItemPickup>();
            if (pickup != null)
                Destroy(pickup);
        }

        Debug.Log($"{weaponItem.displayName} ����!");
    }

    public void Attack()
    {
        // ��ٿ� üũ
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // ���Ⱑ ������ �Ǽ� ����
        if (currentWeapon == null)
        {
            Debug.Log("���Ⱑ �����ϴ�!");
            return;
        }

        lastAttackTime = Time.time;

        // ���� �ִϸ��̼� (�ִٸ�)
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // ������ ���� �ֵθ��� �ִϸ��̼�
        if (currentWeaponObject != null)
        {
            StartCoroutine(SwingWeapon());
        }

        // ������ �� ����
        PerformAttack();
    }

    void PerformAttack()
    {
        // �������� ����ĳ��Ʈ �Ǵ� ������ ���Ǿ�� �� ����
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint, 1f, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}���� {attackDamage} ������!");

            // ������ ������ �ֱ� (�� ��ũ��Ʈ�� �ִٸ�)
            // Enemy enemyScript = enemy.GetComponent<Enemy>();
            // if (enemyScript != null)
            //     enemyScript.TakeDamage(attackDamage);
        }
    }

    System.Collections.IEnumerator SwingWeapon()
    {
        if (currentWeaponObject == null) yield break;

        // ������ �ֵθ��� �ִϸ��̼�
        float swingDuration = 0.3f;
        float elapsed = 0f;

        Vector3 startRot = currentWeaponObject.transform.localEulerAngles;
        Vector3 swingRot = startRot + new Vector3(-90f, 0f, 0f);

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;

            currentWeaponObject.transform.localEulerAngles = Vector3.Lerp(startRot, swingRot, t);
            yield return null;
        }

        // ����ġ�� ����
        elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;

            currentWeaponObject.transform.localEulerAngles = Vector3.Lerp(swingRot, startRot, t);
            yield return null;
        }

        currentWeaponObject.transform.localEulerAngles = startRot;
    }

    void OnDrawGizmosSelected()
    {
        // ���� ���� ǥ��
        Gizmos.color = Color.red;
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackPoint, 1f);
    }
}

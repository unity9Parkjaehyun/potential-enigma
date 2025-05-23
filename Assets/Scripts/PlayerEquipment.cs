using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Equipment Slots")]
    public Transform weaponHolder; // 무기를 들 위치 (오른손)

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

        // 무기 홀더가 없으면 자동으로 찾기
        if (weaponHolder == null)
        {
            Transform rightHand = transform.Find("CameraContainer/RightHand");
            if (rightHand == null)
            {
                // 카메라 컨테이너 아래에 무기 홀더 생성
                GameObject holder = new GameObject("WeaponHolder");
                holder.transform.SetParent(transform.Find("CameraContainer"));
                holder.transform.localPosition = new Vector3(0.5f, -0.3f, 0.7f); // 화면 오른쪽 아래
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
        // 기존 무기 제거
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        // 새 무기 장착
        currentWeapon = weaponItem;

        if (weaponItem.dropPerfab != null)
        {
            currentWeaponObject = Instantiate(weaponItem.dropPerfab, weaponHolder);
            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeaponObject.transform.localRotation = Quaternion.identity;

            // 무기의 Collider를 비활성화 (손에 들고 있을 때는 충돌 방지)
            Collider weaponCollider = currentWeaponObject.GetComponent<Collider>();
            if (weaponCollider != null)
                weaponCollider.enabled = false;

            // ItemPickup 컴포넌트가 있다면 제거
            ItemPickup pickup = currentWeaponObject.GetComponent<ItemPickup>();
            if (pickup != null)
                Destroy(pickup);
        }

        Debug.Log($"{weaponItem.displayName} 장착!");
    }

    public void Attack()
    {
        // 쿨다운 체크
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // 무기가 없으면 맨손 공격
        if (currentWeapon == null)
        {
            Debug.Log("무기가 없습니다!");
            return;
        }

        lastAttackTime = Time.time;

        // 공격 애니메이션 (있다면)
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 간단한 무기 휘두르기 애니메이션
        if (currentWeaponObject != null)
        {
            StartCoroutine(SwingWeapon());
        }

        // 전방의 적 감지
        PerformAttack();
    }

    void PerformAttack()
    {
        // 전방으로 레이캐스트 또는 오버랩 스피어로 적 감지
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint, 1f, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}에게 {attackDamage} 데미지!");

            // 적에게 데미지 주기 (적 스크립트가 있다면)
            // Enemy enemyScript = enemy.GetComponent<Enemy>();
            // if (enemyScript != null)
            //     enemyScript.TakeDamage(attackDamage);
        }
    }

    System.Collections.IEnumerator SwingWeapon()
    {
        if (currentWeaponObject == null) yield break;

        // 간단한 휘두르기 애니메이션
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

        // 원위치로 복귀
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
        // 공격 범위 표시
        Gizmos.color = Color.red;
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackPoint, 1f);
    }
}

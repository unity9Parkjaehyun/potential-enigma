using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpPower = 5f;
    public float staminaCostPerSecond = 20f;
    public float jumpStaminaCost = 30f;

    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public LayerMask interactableLayerMask; // Interactable ���̾� ����ũ �߰�
    private bool isRunning = false;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook = -80f;
    public float maxXLook = 80f;
    private float camCurXRot;
    public float lookSensitivity = 2f;
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;
    private PlayerCondition condition;

    private Inventory inventory;
    private PlayerEquipment equipment;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<PlayerEquipment>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // �޸��� ó��
        float currentSpeed = moveSpeed;
        if (isRunning && condition.HasStamina(staminaCostPerSecond * Time.fixedDeltaTime))
        {
            currentSpeed = runSpeed;
            condition.UseStamina(staminaCostPerSecond * Time.fixedDeltaTime);
        }

        dir *= currentSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() &&
            condition.HasStamina(jumpStaminaCost))
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            condition.UseStamina(jumpStaminaCost);
        }
    }

    // �޸��� �Է� �߰�
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // Ground ���̾�� Interactable ���̾ ��ģ ���̾� ����ũ ����
        LayerMask combinedLayerMask = groundLayerMask | interactableLayerMask;

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 2.0f, combinedLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (inventory != null)
            {
                inventory.ToggleInventory();
            }
        }
    }

    // E Ű - ��ȣ�ۿ�/������ �ݱ�
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // ���� ���� �ִ� ������Ʈ�� ���������� Ȯ��
            Ray ray = cameraContainer.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f, interactableLayerMask))
            {
                // ItemPickup ������Ʈ Ȯ��
                ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.TryPickup();
                    return;
                }

                // �ٸ� ��ȣ�ۿ� ������ ������Ʈ ó��
                // ��: ��, ����ġ ��
            }
        }
    }

    // ���콺 ���� Ŭ�� - ����
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (equipment != null)
            {
                equipment.Attack();
            }
        }
    }
}
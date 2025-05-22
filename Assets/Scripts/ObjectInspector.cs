using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float inspectionRange = 10f;
    public LayerMask interactableLayer = -1;
    public Camera playerCamera;

    [Header("UI References")]
    public InspectionUI inspectionUI;

    [Header("Debug")]
    public bool showDebugRay = true;

    private InteractableObject currentObject;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (inspectionUI == null)
            inspectionUI = FindObjectOfType<InspectionUI>();
    }

    void Update()
    {
        PerformRaycast();
        HandleInput();
    }

    void PerformRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, inspectionRange, interactableLayer))
        {
            InteractableObject hitObject = hit.collider.GetComponent<InteractableObject>();

            if (hitObject != null)
            {
                // ���ο� ������Ʈ�� �ٶ󺸰� �ִ� ���
                if (currentObject != hitObject)
                {
                    // ���� ������Ʈ ���̶���Ʈ ����
                    if (currentObject != null)
                    {
                        currentObject.RemoveHighlight();
                    }

                    // �� ������Ʈ ���̶���Ʈ
                    currentObject = hitObject;
                    currentObject.Highlight();

                    // UI ������Ʈ
                    if (inspectionUI != null)
                    {
                        inspectionUI.ShowObjectInfo(currentObject.GetObjectInfo());
                    }
                }
            }
            else
            {
                ClearCurrentObject();
            }
        }
        else
        {
            ClearCurrentObject();
        }

        // ����׿� ���� �׸���
        if (showDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * inspectionRange, Color.red);
        }
    }

    void ClearCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.RemoveHighlight();
            currentObject = null;

            if (inspectionUI != null)
            {
                inspectionUI.HideObjectInfo();
            }
        }
    }

    void HandleInput()
    {
        // EŰ�� �� ���� ���
        if (Input.GetKeyDown(KeyCode.E) && currentObject != null && inspectionUI != null)
        {
            inspectionUI.ToggleDetailedInfo();
        }
    }

    public InteractableObject GetCurrentObject()
    {
        return currentObject;
    }
}

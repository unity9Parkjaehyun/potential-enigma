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
                // 새로운 오브젝트를 바라보고 있는 경우
                if (currentObject != hitObject)
                {
                    // 이전 오브젝트 하이라이트 제거
                    if (currentObject != null)
                    {
                        currentObject.RemoveHighlight();
                    }

                    // 새 오브젝트 하이라이트
                    currentObject = hitObject;
                    currentObject.Highlight();

                    // UI 업데이트
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

        // 디버그용 레이 그리기
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
        // E키로 상세 정보 토글
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

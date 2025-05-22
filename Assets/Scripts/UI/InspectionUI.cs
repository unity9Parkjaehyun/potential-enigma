using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject inspectionPanel;
    public TextMeshProUGUI objectNameText;
    public TextMeshProUGUI objectDescriptionText;
    public Image objectIcon;
    public GameObject detailedInfoPanel;
    public GameObject crosshair; // ũ�ν���� �߰�

    [Header("Animation Settings")]
    public float fadeSpeed = 3f;

    private CanvasGroup panelCanvasGroup;
    private bool isDetailedInfoVisible = false;

    void Start()
    {
        panelCanvasGroup = inspectionPanel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = inspectionPanel.AddComponent<CanvasGroup>();
        }

        HideObjectInfo();

        // ũ�ν���� Ȱ��ȭ
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    public void ShowObjectInfo(ObjectInfo info)
    {
        // �⺻ ���� ������Ʈ
        if (objectNameText != null)
        {
            objectNameText.text = info.objectName;
        }

        if (objectIcon != null && info.icon != null)
        {
            objectIcon.sprite = info.icon;
            objectIcon.gameObject.SetActive(true);
        }
        else if (objectIcon != null)
        {
            objectIcon.gameObject.SetActive(false);
        }

        // �г� ǥ��
        inspectionPanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void HideObjectInfo()
    {
        if (inspectionPanel.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }

        if (detailedInfoPanel != null)
        {
            detailedInfoPanel.SetActive(false);
            isDetailedInfoVisible = false;
        }
    }

    public void ToggleDetailedInfo()
    {
        if (detailedInfoPanel != null)
        {
            isDetailedInfoVisible = !isDetailedInfoVisible;
            detailedInfoPanel.SetActive(isDetailedInfoVisible);

            if (isDetailedInfoVisible)
            {
                ObjectInspector inspector = FindObjectOfType<ObjectInspector>();
                if (inspector != null)
                {
                    var currentObj = inspector.GetCurrentObject();
                    if (currentObj != null && objectDescriptionText != null)
                    {
                        objectDescriptionText.text = currentObj.GetObjectInfo().description;
                    }
                }
            }
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        panelCanvasGroup.alpha = 0f;
        while (panelCanvasGroup.alpha < 1f)
        {
            panelCanvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        while (panelCanvasGroup.alpha > 0f)
        {
            panelCanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        panelCanvasGroup.alpha = 0f;
        inspectionPanel.SetActive(false);
    }
}
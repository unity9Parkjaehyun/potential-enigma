using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public string objectName;
    [TextArea(2, 4)]
    public string description;
    public Sprite icon;
}

public class InteractableObject : MonoBehaviour
{
    [Header("Object Information")]
    public ObjectInfo objectInfo;

    [Header("Interaction Settings")]
    public float interactionDistance = 5f;

    // 하이라이트 효과용
    private Renderer objectRenderer;
    private Material originalMaterial;
    private Material highlightMaterial;
    private bool isHighlighted = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;

            // 하이라이트 머티리얼 생성
            highlightMaterial = new Material(originalMaterial);
            highlightMaterial.color = Color.yellow;
            highlightMaterial.SetFloat("_Metallic", 0.5f);
            highlightMaterial.SetFloat("_Smoothness", 0.8f);
        }
    }

    public void Highlight()
    {
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material = highlightMaterial;
            isHighlighted = true;
        }
    }

    public void RemoveHighlight()
    {
        if (objectRenderer != null && isHighlighted)
        {
            objectRenderer.material = originalMaterial;
            isHighlighted = false;
        }
    }

    public ObjectInfo GetObjectInfo()
    {
        return objectInfo;
    }

    private void OnDestroy()
    {
        if (highlightMaterial != null)
        {
            DestroyImmediate(highlightMaterial);
        }
    }
}
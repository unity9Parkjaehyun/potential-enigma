using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    // �̺�Ʈ �߰�
    public UnityEvent<float> OnValueChanged;
    public UnityEvent OnZero;

    private float lastValue;

    private void Start()
    {
        curValue = startValue;
        lastValue = curValue;
        UpdateUI();
    }

    // Update���� �Ź� UI ������Ʈ���� �ʰ� ���� ����� ���� ������Ʈ
    private void Update()
    {
        // ���� ����Ǿ��� ���� UI ������Ʈ
        if (Mathf.Abs(curValue - lastValue) > 0.01f)
        {
            UpdateUI();
            lastValue = curValue;
        }

        // Passive �� ���� (�ð��� ���� �ڵ� ����/����)
        if (passiveValue != 0)
        {
            Add(passiveValue * Time.deltaTime);
        }
    }

    private void UpdateUI()
    {
        if (uiBar != null)
        {
            uiBar.fillAmount = GetPercentage();
        }

        OnValueChanged?.Invoke(curValue);

        if (curValue <= 0)
        {
            OnZero?.Invoke();
        }
    }

    public void Add(float amount)
    {
        float newValue = Mathf.Min(curValue + amount, maxValue);
        if (newValue != curValue)
        {
            curValue = newValue;
        }
    }

    public void Subtract(float amount)
    {
        float newValue = Mathf.Max(curValue - amount, 0.0f);
        if (newValue != curValue)
        {
            curValue = newValue;
        }
    }

    public float GetPercentage()
    {
        return maxValue > 0 ? curValue / maxValue : 0;
    }

    public bool IsZero()
    {
        return curValue <= 0;
    }

    public bool IsFull()
    {
        return curValue >= maxValue;
    }
}
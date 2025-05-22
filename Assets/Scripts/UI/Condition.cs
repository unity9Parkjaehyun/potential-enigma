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

    // 이벤트 추가
    public UnityEvent<float> OnValueChanged;
    public UnityEvent OnZero;

    private float lastValue;

    private void Start()
    {
        curValue = startValue;
        lastValue = curValue;
        UpdateUI();
    }

    // Update에서 매번 UI 업데이트하지 않고 값이 변경될 때만 업데이트
    private void Update()
    {
        // 값이 변경되었을 때만 UI 업데이트
        if (Mathf.Abs(curValue - lastValue) > 0.01f)
        {
            UpdateUI();
            lastValue = curValue;
        }

        // Passive 값 적용 (시간에 따른 자동 감소/증가)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIContidition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.Stamina; } }

    private void Start()
    {

    }

    private void Update()
    {
        // ���¹̳� �ڵ� ȸ�� (����)
        if (stamina != null && !stamina.IsFull())
        {
            stamina.Add(10f * Time.deltaTime); // �ʴ� 10�� ȸ��
        }
    }

    public void TakeDamage(float damage)
    {
        if (health != null)
        {
            health.Subtract(damage);
        }
    }

    public void UseStamina(float amount)
    {
        if (stamina != null)
        {
            stamina.Subtract(amount);
        }
    }

    public bool HasStamina(float amount)
    {
        return stamina != null && stamina.curValue >= amount;
    }

    public void Die()
    {
        Debug.Log("�÷��̾ �׾����ϴ�!");
        // �߰� ��� ó�� ����
        Time.timeScale = 0; // ���� �Ͻ����� (����)
    }
}
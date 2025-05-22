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
        // 스태미나 자동 회복 (예시)
        if (stamina != null && !stamina.IsFull())
        {
            stamina.Add(10f * Time.deltaTime); // 초당 10씩 회복
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
        Debug.Log("플레이어가 죽었습니다!");
        // 추가 사망 처리 로직
        Time.timeScale = 0; // 게임 일시정지 (예시)
    }
}
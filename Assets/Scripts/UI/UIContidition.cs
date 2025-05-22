using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContidition : MonoBehaviour
{

    public Condition health;
    public Condition Stamina;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.player.condition.uiCondition = this;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCharacter : Character
{
    

    public override void TakeDamage(Attack attacker, bool attackType=false)
    {
        base.TakeDamage(attacker, attackType);
        EventCenter.Instance.TriggerEvent("STHpChange", this);
    }
}

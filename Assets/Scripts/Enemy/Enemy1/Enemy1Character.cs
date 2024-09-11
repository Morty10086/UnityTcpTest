using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Character : Character
{  

    public override void TakeDamage(Attack attacker,bool attackType)
    {
        base.TakeDamage(attacker,attackType);
        this.GetComponentInChildren<EnmeyHp>().EnmeyHpChange(this);
    }
}

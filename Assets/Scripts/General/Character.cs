using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("���˺������¼�")]
    public UnityEvent<Transform,bool> hurtEvent;
    public UnityEvent deadEvent;

    [Header("��������")]
    public float maxHp;
    public float currentHp;
    [Header("�����޵�")]
    public bool isCanInvincible;
    public float invincibleTime;
    public float invincibleCounter;
    public bool isInvincible;

    protected virtual void OnEnable()
    {
        currentHp = maxHp;

    }
    protected virtual void Update()
    {
        if (isCanInvincible&&isInvincible)
        {
            invincibleCounter-=Time.deltaTime;
            if(invincibleCounter<=0)
            {
                isInvincible = false;
            }
        }
    }
    public virtual void TakeDamage(Attack attacker,bool attackType=false)
    {
        if (isInvincible)
            return;
        if(currentHp-attacker.damage > 0)
        {
            currentHp-=attacker.damage;
            this.TriggerInvincible();
            hurtEvent?.Invoke(attacker.gameObject.transform,attackType);
        }
        else
        {
            currentHp = 0;
            deadEvent?.Invoke();
        }
        EventCenter.Instance.TriggerEvent("CameraShake", null);
    }
    
    //�����޵к���
    public virtual void TriggerInvincible()
    {
        if(isCanInvincible&&!isInvincible)
        {
            isInvincible = true;
            invincibleCounter = invincibleTime;
        }
    }
}

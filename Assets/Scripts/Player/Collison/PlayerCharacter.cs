using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private bool hasDead;
    public PlayerHpPanel hpPanel;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.hpPanel.hpImage.fillAmount = this.currentHp / this.maxHp;
    }
    protected override void Update()
    {
        base.Update();
        this.hpPanel.hpImage.fillAmount = this.currentHp / this.maxHp;
    }
    public override void TakeDamage(Attack attacker, bool attackType=false)
    {
        if (isInvincible)
            return;
        if (currentHp - attacker.damage > 0)
        {
            currentHp -= attacker.damage;
            this.TriggerInvincible();
            hurtEvent?.Invoke(attacker.gameObject.transform,attackType);
        }
        else
        {
            currentHp = 0;
            if (!hasDead)
            {
                hasDead = true;
                deadEvent?.Invoke();
            }
            
        }

        this.hpPanel.hpImage.fillAmount = this.currentHp / this.maxHp;
        EventCenter.Instance.TriggerEvent("CameraShake", null);
    }
}

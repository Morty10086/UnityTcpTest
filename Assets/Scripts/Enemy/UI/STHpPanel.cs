using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STHpPanel : BasePanel
{
    public Image hpFadeImage;
    public Image hpImage;

    public override void Init()
    {
       
    }
    protected override void Update()
    {
        base.Update();
        if (hpFadeImage.fillAmount > hpImage.fillAmount)
        {
            hpFadeImage.fillAmount -= Time.deltaTime*0.3f;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        EventCenter.Instance.AddEventListener("STHpChange", STHpChange);
      
    }

    public void STHpChange(object info)
    {
        this.hpImage.fillAmount = (info as Character).currentHp / (info as Character).maxHp;
    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("STHpChange", STHpChange);
    }
}

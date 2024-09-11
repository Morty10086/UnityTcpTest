using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerHpPanel : BasePanel
{
    public Image hpFadeImage;
    public Image hpImage;
    public Image timeImage;
    public Image[] bulletImages;

    protected override void Awake()
    {
        base.Awake();
        EventCenter.Instance.AddEventListener("HpChange", HpChange);
        EventCenter.Instance.AddEventListener("TimeChange", TimeChange);
        EventCenter.Instance.AddEventListener("BulletChange", BulletChange);
    }
    public override void Init()
    {
        
    }


    protected override void Update()
    {
        base.Update();
        if(hpFadeImage.fillAmount>hpImage.fillAmount)
        {
            hpFadeImage.fillAmount -= Time.deltaTime;
        }
    }
    public void HpChange(object info)
    {
        this.hpImage.fillAmount=(info as Character).currentHp/ (info as Character).maxHp;
    }
    public void TimeChange(object info)
    {
        this.timeImage.fillAmount = (info as PlayerController).stopTimeCounter / (info as PlayerController).stopTimeNeed;
    }

    public void BulletChange(object info)
    {
        int index = (info as PlayerController).shootCounter;
        if(index>= (info as PlayerController).maxShootCount)
        {
            foreach(Image bulletImage in bulletImages)
            {
                bulletImage.gameObject.SetActive(true);
            }
        }
        else
        {
            bulletImages[index].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("HpChange", HpChange);
        EventCenter.Instance.RemoveEvent("TimeChange", TimeChange);
        EventCenter.Instance.RemoveEvent("BulletChange", BulletChange);
    }
}

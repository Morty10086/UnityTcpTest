using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnmeyHp : MonoBehaviour
{
    public Transform targetTrans;
    public Image[] hpImages;
    public Vector3 offset;
    public Image hpImage;

    private void Awake()
    {
        
    }
    void Update()
    {
        foreach (Image hp in hpImages) 
        {
            hp.transform.position = Camera.main.WorldToScreenPoint(targetTrans.position + offset);
        }
        
    }

    public void EnmeyHpChange(Character info)
    {
        if(hpImage!=null)
            this.hpImage.fillAmount = info.currentHp /info.maxHp;
    }
}

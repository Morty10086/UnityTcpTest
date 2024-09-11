using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAwakeTimeLine : TriggerTimeLine
{
    private PlayerController pCotroller;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pCotroller = other.GetComponent<PlayerController>();
           
            if (isOnce && !GameDataMgr.Instance.awakeTimeLine)
            {
                pCotroller.playerInput.Disable();
                GameDataMgr.Instance.awakeTimeLine = true;
                timeLineDirector.SetActive(true);
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStoreForceDialogue : MonoBehaviour
{
    public GameObject stopTimeTip;
    public GameObject airWall;
    private bool hasTrigger;
    private void Awake()
    {
        if (GameDataMgr.Instance.inStoreTimeLine)
        {
            airWall.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&GameDataMgr.Instance.inStoreTimeLine&&!hasTrigger)
        {
            hasTrigger = true;
            stopTimeTip.SetActive(true);
            EventCenter.Instance.TriggerEvent("TriggerDialogue", null);
        }
    }
}

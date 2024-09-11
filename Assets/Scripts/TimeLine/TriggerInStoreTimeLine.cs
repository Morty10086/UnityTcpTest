using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInStoreTimeLine :TriggerTimeLine
{
    private PlayerController pController;
    public GameObject dialogueTL;
    public GameObject dialogueTLF;
    public GameObject dialogue;
    private void Awake()
    {
        if (GameDataMgr.Instance.inStoreTimeLine)
        {
            dialogueTL.SetActive(false);
            dialogueTLF.SetActive(false);
            dialogue.SetActive(true);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pController = other.GetComponent<PlayerController>();
            if (isOnce && !GameDataMgr.Instance.inStoreTimeLine)
            {
                pController.playerInput.Disable();
                GameDataMgr.Instance.inStoreTimeLine = true;
                timeLineDirector.SetActive(true);
            }

        }
    }
}

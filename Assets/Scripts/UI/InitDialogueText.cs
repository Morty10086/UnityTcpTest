using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDialogueText : MonoBehaviour
{
    private DialogueUIMgr dUIMgr;
    private void Awake()
    {
        dUIMgr=this.GetComponentInParent<DialogueUIMgr>();
    }
    private void OnEnable()
    {
        if (dUIMgr.isFinished)
        {
            dUIMgr.InitTextAsset(dUIMgr.textAsset);
        }
        else
        {
            dUIMgr.InitTextAsset(dUIMgr.finishedTextAsset);
        }
        
    }
}

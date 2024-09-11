using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayOnEnable : MonoBehaviour
{
    public AudioID id;
    public AudioClip warnningClip;
    public bool disStop;
    public bool stWarnning;
    private void OnEnable()
    {
        if (stWarnning)
            AudioMgr.Instance.PlaySound(warnningClip,false,false,0.3f);
        else
            AudioMgr.Instance.PlaySoundNew(id);
    }
    private void OnDisable()
    {
        if (disStop)
            AudioMgr.Instance.StopSoundNew(id);
    }
}

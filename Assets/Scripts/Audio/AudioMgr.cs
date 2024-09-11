using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioID
{
    playerMove,
    playerJump,
    playerDash,
    playerA1,
    playerA2,
    playerA3,
    playerA4,
    playerAA,
    playerS,
    playerAS,
    playerST,
    pHurtBlade,
    pHurtBullet,
    mHurtBlade,
    mHurtBullet,
}
public class AudioMgr:MonoBehaviour
{
    static private AudioMgr instance;
    static public AudioMgr Instance=>instance;

    public AudioSource audioSource;
    public AudioSource[] audioSources;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void PlaySound(AudioClip clip,bool isMove=false,bool isLoop=false,float soundValue=1)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.volume = soundValue;
        if (isMove)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Play();
        }
            
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
    public void PlaySoundNew(AudioID id)
    {
        if (id == AudioID.playerMove)
        {
            if (!audioSources[(int)id].isPlaying)
                audioSources[(int)id].Play();
        }
        else
        {
            audioSources[(int)id].Play();
        }
        //switch (id)
        //{
        //    case AudioID.playerMove:
        //        audioSources[0].Play();
        //        break;
        //    case AudioID.playerJump:
        //        audioSources[1].Play(); 
        //        break;
        //    case AudioID.playerDash:
        //        audioSources[2].Play();
        //        break;
        //    case AudioID.playerA1:
        //        audioSources[3].Play();
        //        break;
        //    case AudioID.playerA2:
        //        audioSources[4].Play();
        //        break;
        //    case AudioID.playerA3:
        //        audioSources[5].Play();
        //        break;
        //    case AudioID.playerA4:
        //        audioSources[6].Play();
        //        break;
        //    case AudioID.playerAA:
        //        audioSources[7].Play();
        //        break;
        //    case AudioID.playerS:
        //        audioSources[8].Play();
        //        break;
        //    case AudioID.playerAS:
        //        audioSources[9].Play();
        //        break;
        //    case AudioID.playerST:
        //        audioSources[10].Play();
        //        break;
        //}
    }
    public void StopSoundNew(AudioID id)
    {
        audioSources[(int)id].Stop();
    }
}

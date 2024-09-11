using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAudioSource : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSourceLoop;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("PlaySound", PlaySound);
    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("PlaySound", PlaySound);
    }
    private void PlaySound(object info)
    {
        audioSource.clip=info as AudioClip;
        audioSource.Play();
    }

    private void PlaySoundLoop(object info)
    {
        audioSourceLoop.clip = info as AudioClip;
        audioSourceLoop.Play();
    }
}

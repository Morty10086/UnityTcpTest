using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Sound : MonoBehaviour
{
    public AudioSource audioSource;
    private Enemy enemy;
    private void OnEnable()
    {
        enemy=this.GetComponentInParent<Enemy>();
        audioSource = GetComponentInParent<AudioSource>();
    }
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip attackClip;
    public AudioClip deadClip;

    private void Update()
    {
        if (enemy.beStop&&(audioSource.clip==walkClip||audioSource.clip==runClip))
        {
            StopSound();
        }
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
    public void PlayWalkSound()
    {
        audioSource.clip = walkClip; 
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void PlayRunSound()
    {
        audioSource.clip = runClip;
        if (!audioSource.isPlaying)
        {         
            audioSource.Play();
        }
    }
    public void PlayAttackSound()
    {
        audioSource.clip = attackClip;
        audioSource.Play();
    }
    public void PlayDeadSound()
    {
        audioSource.clip = deadClip;
        audioSource.Play();
    }
}

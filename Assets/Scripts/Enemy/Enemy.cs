using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioSource audioSource;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Collider collider;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PhysicsCheck check;
    [HideInInspector]
    public BaseState currentState;
    [Header("受击")]
    public float hurtForce;
    public AudioClip bladeClip;
    public AudioClip bulletClip;
    [Header("时停相关")]
    public GameObject stopTimeRange;
    public bool isInRange;
    public bool beStop;
    protected virtual void Awake()
    {
        rb= GetComponent<Rigidbody>();
        collider= GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();     
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void GetHurt(Transform attackerTrans,bool attackType = false )
    {

    }
    protected virtual void PlayHurtSound(bool attackType)
    {
        if (attackType)
        {
            audioSource.clip = bulletClip;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = bladeClip;
            audioSource.Play();
        }

    }
    public virtual void GetDead()
    {

    }

    protected virtual void SetAnimation()
    {

    }

    //时停：
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopTime"))
        {
            isInRange = true;
            stopTimeRange = other.gameObject;
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("StopTime"))
        {
            isInRange = true;
            stopTimeRange = other.gameObject;
        }
    }

    protected virtual void EndRangeTimeStop()
    {
        if (stopTimeRange != null)
        {
            if (!stopTimeRange.activeSelf)
            {
                isInRange = false;
            }
        }
    }
    protected virtual void StopTimeDo()
    {

    }
}

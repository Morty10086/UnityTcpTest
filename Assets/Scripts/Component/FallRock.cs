using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRock : BaseGameLevel
{
    public float speed;
    public float currentSpeed;
    public int direction = -1;
    public AudioSource audioSource;
    private PlayerController pController;
    private bool hasPlaySound;

    void Update()
    {
        RockFall();

        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            currentSpeed = speed;
        }

        EndRangeTimeStop();
    }

    private void OnEnable()
    {
        hasPlaySound = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (!isInRange && !PlayerController.isStopTime && collision.gameObject.CompareTag("Player"))
        {            
            pController = collision.gameObject.GetComponent<PlayerController>();
            collision.gameObject.GetComponent<Character>().TakeDamage(this);
            PoolManager.Instance.PushObj("Component/GameLevel/FallRock", this.gameObject);
        }

        
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
            return;
        if (other.CompareTag("Ground"))
        {
            if (!hasPlaySound)
            {
                hasPlaySound=true;
                audioSource.Play();
                Invoke("DistroyThis", 1f);
            }             
        }
    }

    void DistroyThis()
    {
        PoolManager.Instance.PushObj("Component/GameLevel/FallRock", this.gameObject);
    }
    private void RockFall()
    {
        this.transform.Translate(Vector3.up * currentSpeed * Time.deltaTime * direction, Space.World);
    }
    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformV : BaseGameLevel
{
    public float speed;
    public float maxMoveDistance;
    public float nowMoveDistance;
    public float currentSpeed;
    public int direction = 1;
    public bool isCanMove;
    public Vector3 displacement;
    public AudioSource audioSource;

    private PlayerController pController;
    public bool dontReturnPlayer;
    public bool dontDamage;

    void Update()
    {
        if (isCanMove)
        {
            PlatformMove();
        }

        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            audioSource.Stop();
            StopTimeDo();
        }
        else
        {
            if(isCanMove&&!audioSource.isPlaying)
                audioSource.Play();
            currentSpeed = speed;
        }

        EndRangeTimeStop();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isInRange && !PlayerController.isStopTime && collision.gameObject.CompareTag("Player"))
        {
            pController=collision.gameObject.GetComponent<PlayerController>();          
            if (pController.phyCheck.isGround&&direction == -1&&this.transform.position.y> collision.gameObject.transform.position.y)
            {
                if(!dontDamage)
                    collision.gameObject.GetComponent<Character>().TakeDamage(this);
                //if(!dontReturnPlayer)
                    //pController.playerReturnCheckPoint();
            }
        }
    }

    public void PlatformMove()
    {
        this.transform.Translate(this.transform.up * currentSpeed * Time.deltaTime * direction, Space.World);
        nowMoveDistance+= currentSpeed * Time.deltaTime;
        displacement = this.transform.up * currentSpeed * Time.deltaTime * direction;
        if (nowMoveDistance>=maxMoveDistance)
        {
            nowMoveDistance = 0;
            direction *= -1;
        }

    }
    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }
}

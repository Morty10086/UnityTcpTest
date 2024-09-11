using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformH : BaseGameLevel
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float maxMoveDistance;
    public float nowMoveDistance;
    public float speed;
    public float currentSpeed;
    public int direction=1;
    public bool isCanMove;
    public Vector3 displacement;
    public AudioSource audioSource;
   
    void Update()
    {
        if(isCanMove)
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
   
    public void PlatformMove()
    {
        this.transform.Translate(this.transform.right * currentSpeed * Time.deltaTime * direction, Space.World);
        nowMoveDistance += currentSpeed * Time.deltaTime;
        displacement = this.transform.right * currentSpeed * Time.deltaTime * direction;
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

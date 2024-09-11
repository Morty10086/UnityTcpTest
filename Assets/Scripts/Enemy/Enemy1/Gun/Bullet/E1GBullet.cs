using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1GBullet : BaseGameLevel
{
    public int flyDir;
    public float flySpeed;
    public float currentSpeed;

    public float boomTime;
    public float boomTimeCounter;
    private void OnEnable()
    {
        boomTimeCounter = 0;
    }
    private void Update()
    {
        EndRangeTimeStop();
        BulletFly();
        boomTimeCounter += Time.deltaTime;
        if((isInRange&&PlayerController.isRangeStopTime)||PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            currentSpeed = flySpeed;
        }
        if (boomTimeCounter >= boomTime)
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/E1GBullet", this.gameObject);
        }
    }

    private void BulletFly()
    {
        this.transform.Translate(Vector3.right*flyDir*currentSpeed*Time.deltaTime,Space.Self);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character>()?.TakeDamage(this,true);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/E1GBullet", this.gameObject);
        }

        if(other.CompareTag("Ground"))
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/E1GBullet",this.gameObject);
        }
    }
    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }

}

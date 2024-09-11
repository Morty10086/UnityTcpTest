using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet :BaseGameLevel
{
    public float flySpeed;
    public float currentSpeed;

    public float boomTime;
    public float boomTimeCounter;
    private void OnEnable()
    {
        boomTimeCounter = 0;
        this.transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        EndRangeTimeStop();
        BulletFly();
        boomTimeCounter += Time.deltaTime;
        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            currentSpeed = flySpeed;
        }
        if (boomTimeCounter >= boomTime)
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/DroneBullet", this.gameObject);
        }
    }

    private void BulletFly()
    {
        this.transform.Translate(Vector3.right * currentSpeed * Time.deltaTime, Space.Self);
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
            other.GetComponent<Character>()?.TakeDamage(this, true);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/DroneBullet", this.gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/DroneBullet", this.gameObject);
        }
    }
    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }

}

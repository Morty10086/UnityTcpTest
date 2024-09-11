using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STRocket : BaseGameLevel
{
    public float landSpeed;
    private float currentSpeed;
    private GameObject boomEffect;

    void Update()
    {
        EndRangeTimeStop();
        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            currentSpeed = landSpeed;
        }

        this.transform.Translate(Vector3.down * currentSpeed * Time.deltaTime, Space.World);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Invincible"))
        {
            boomEffect = PoolManager.Instance.GetObj("Bullet/EnemyBullet/RocketBoom");
            boomEffect.transform.position = this.transform.position;
            Invoke("PushEffect", 1f);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/STRocket", this.gameObject);
        }

        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character>()?.TakeDamage(this, true);
            boomEffect = PoolManager.Instance.GetObj("Bullet/EnemyBullet/RocketBoom");
            boomEffect.transform.position = this.transform.position;
            Invoke("PushEffect", 1f);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/STRocket", this.gameObject);
        }
    }

    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }

    private void PushEffect()
    {
        if (boomEffect != null)
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/RocketBoomSmall", boomEffect);
        }
    }

}

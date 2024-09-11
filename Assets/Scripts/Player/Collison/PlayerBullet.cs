using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseGameLevel
{
    public int flyDir;
    public float flySpeed;
    public float currentSpeed;
    public GameObject player;

    public float boomTime;
    public float boomTimeCounter;
    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        boomTimeCounter = 0;
    }
    private void Update()
    {
        boomTime += Time.deltaTime;
        EndRangeTimeStop();
        BulletFly();

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
            PoolManager.Instance.PushObj("Bullet/PlayerBullet", this.gameObject);
        }
    }

    private void BulletFly()
    {
        this.transform.Translate(Vector3.right * flyDir * currentSpeed * Time.deltaTime, Space.Self);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Character>()?.TakeDamage(this,true);
            PoolManager.Instance.PushObj("Bullet/PlayerBullet", this.gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            PoolManager.Instance.PushObj("Bullet/PlayerBullet", this.gameObject);
        }
    }
    protected override void StopTimeDo()
    {
        currentSpeed = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class STBullet : BaseGameLevel
{

    private Transform target;
    public float aimTime;
    public float angle;
    public float flyDir;
    private Vector3 lookDirection;
    public float flySpeed;
    public float currentSpeed;
    public Vector2 targetOffset;
    private float timeCounter;
    private bool canAim;

    private Vector3 startRotation;
    public float rotateSpeed;
    public bool isStage2;

    private GameObject boomEffect;
    public float boomTime;
    public float boomTimeCounter;
    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        canAim = true;
    }

    private void OnEnable()
    {
        target = GameObject.Find("Player").transform;
        canAim = true;     
        timeCounter = 0;
        boomTimeCounter = 0;
    }

   
    void Update()
    {
        boomTimeCounter += Time.deltaTime;
        EndRangeTimeStop();
        if((isInRange&&PlayerController.isRangeStopTime)||PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            
            currentSpeed = flySpeed;
            if (canAim)
            {
                this.transform.right = Vector3.RotateTowards(this.transform.right, target.position  - this.transform.position,rotateSpeed*Time.deltaTime,0);
                //lookDirection = target.position + (Vector3)targetOffset - this.transform.position;
                //this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                //this.transform.rotation *= Quaternion.Euler(0, 0, angle * flyDir); 
            }
            this.transform.Translate(this.transform.right * currentSpeed * Time.deltaTime, Space.World);
        }
        if (boomTimeCounter >= boomTime)
        {
            boomEffect = PoolManager.Instance.GetObj("Bullet/EnemyBullet/RocketBoomSmall");
            boomEffect.transform.position = this.transform.position;
            Invoke("PushEffect", 1f);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/STRocketSmall", this.gameObject);
        }
    }

    IEnumerator AimTarget()
    {
        while(this.gameObject.activeSelf)
        {          
            if (target.position.y + targetOffset.y < this.transform.position.y)
            {
                lookDirection = target.position + (Vector3)targetOffset - this.transform.position;
                this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                this.transform.rotation *= Quaternion.Euler(0, 0, angle * flyDir); ;
            }
            yield return new WaitForSeconds(aimTime);
        }      
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (/*other.CompareTag("MovePlatform") ||*/ other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Invincible"))
        {
            boomEffect = PoolManager.Instance.GetObj("Bullet/EnemyBullet/RocketBoomSmall");
            boomEffect.transform.position = this.transform.position;
            Invoke("PushEffect", 1f);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/STRocketSmall", this.gameObject);
        }

        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character>()?.TakeDamage(this,true);
            boomEffect = PoolManager.Instance.GetObj("Bullet/EnemyBullet/RocketBoomSmall");
            boomEffect.transform.position = this.transform.position;
            Invoke("PushEffect", 1f);
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/STRocketSmall",this.gameObject);
        }
    }


    protected override void StopTimeDo()
    {
        currentSpeed = 0;
        canAim=false;
    }

    private void PushEffect()
    {
        if (boomEffect != null)
        {
            PoolManager.Instance.PushObj("Bullet/EnemyBullet/RocketBoomSmall",boomEffect);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1G : Enemy
{
    private Dictionary<E1GStateEnum, BaseState> stateDic = new Dictionary<E1GStateEnum, BaseState>();
    public E1GPhysicsCheck e1gPhyCheck;
    public GameObject hurtEffectBlade;
    public GameObject hurtEffectBullet;

    [Header("转向等待与脱战设置")]
    public bool isIdle = false;
    public float idleTime;
    public float outCombatTime;
    [Header("移动")]
    public float moveDir;
    public float walkSpeed;
    public float runSpeed;
    [Header("巡逻")]
    public Transform[] walkPoints;
    [Header("追击")]
    public Transform[] runPoints;
    public float runDistance;
    public float safeDistance;
    public bool isFindPlayer;
    public Transform playerTrans;
    [Header("攻击")]
    public bool isCanAttack;
    [Header("受击")]
    public bool isHurt;
    public bool isDead;
    protected override void Awake()
    {
        base.Awake();
        stateDic.Add(E1GStateEnum.Idle, new E1GIdle());
        stateDic.Add(E1GStateEnum.Walk, new E1GWalk());
        stateDic.Add(E1GStateEnum.Run, new E1GRun());
        stateDic.Add(E1GStateEnum.Shoot, new E1GShoot());
        stateDic.Add(E1GStateEnum.Hurt, new E1GHurt());
        stateDic.Add(E1GStateEnum.Dead, new E1GDead());
    }
    private void OnEnable()
    {
        SwitchState(E1GStateEnum.Idle);
    }
    void Start()
    {
        
    }

    void Update()
    {
        moveDir = this.transform.localScale.x;
        EndRangeTimeStop();   
        if ((isInRange && PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            beStop = false;
            animator.speed = 1;
        }
        currentState.LogicUpdate();
    }

    public void SwitchState(E1GStateEnum e1gStateNum)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = stateDic[e1gStateNum];
        currentState.OnEnter(this);
    }

    public void FlipTo(Transform tagetTrans,bool isRun=false)
    {

        if(tagetTrans!=null)
        {
            if(isRun)
            {
                float dir = tagetTrans.position.x - this.transform.position.x;
                this.transform.localScale = new Vector3(dir > 0 ? -1 : 1, 1, 1);
            }
            else
            {
                float dir = tagetTrans.position.x - this.transform.position.x;
                this.transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
            }
        }
    }
    public override void GetHurt(Transform attackerTrans, bool attackType = false)
    {
        PlayHurtSound(attackType);
        if (attackType)
            hurtEffectBullet.SetActive(true);
        else
            hurtEffectBlade.SetActive(true);
        StartCoroutine(HurtEffect());
        if (beStop)
            return;
        this.isHurt = true;
        Vector2 dir = new Vector2(this.transform.position.x - attackerTrans.position.x, 0).normalized;
        this.rb.velocity = dir * hurtForce;
    }
    private IEnumerator HurtEffect()
    {
        yield return new WaitForSeconds(0.1f);
        hurtEffectBlade.SetActive(false);
        hurtEffectBullet.SetActive(false);
    }
    public override void GetDead()
    {
        this.isDead = true;
        animator.speed = 1;
        this.gameObject.layer = LayerMask.NameToLayer("Dead");
        Invoke("DestroyThis", 1);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    protected override void StopTimeDo()
    {
        beStop = true;
        if (!isDead)
            animator.speed = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1B :Enemy
{
    private Dictionary<E1BStateEnum, BaseState> stateDic = new Dictionary<E1BStateEnum, BaseState>();
    public E1BPhysicsCheck e1bPhyCheck;
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
        stateDic.Add(E1BStateEnum.Idle, new E1BIdle());
        stateDic.Add(E1BStateEnum.Walk, new E1BWalk());
        stateDic.Add(E1BStateEnum.Run, new E1BRun());
        stateDic.Add(E1BStateEnum.Attack, new E1BCut());
        stateDic.Add(E1BStateEnum.Hurt, new E1BHurt());
        stateDic.Add(E1BStateEnum.Dead, new E1BDead());
    }
    
    private void OnEnable()
    {
        SwitchState(E1BStateEnum.Idle);
    }
    private void Update()
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
    public void SwitchState(E1BStateEnum e1bStateNum)
    {
        if(currentState!=null)
            currentState.OnExit();
        currentState = stateDic[e1bStateNum];
        currentState.OnEnter(this);
    }

    public void FlipTo(Transform tagetTrans)
    {
        if(tagetTrans!=null)
        {
            float dir = tagetTrans.position.x - this.transform.position.x;
            this.transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
        }      
    }

    public override void GetHurt(Transform attackerTrans,bool attackType)
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
        yield return new WaitForSeconds(0.2f);
        hurtEffectBlade.SetActive (false);
        hurtEffectBullet.SetActive (false);
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

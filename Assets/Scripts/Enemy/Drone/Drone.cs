using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Drone : Enemy
{
    private Dictionary<DroneStateEnum, BaseState> stateDic = new Dictionary<DroneStateEnum, BaseState>();
    
    [Header("Idle")]
    public float idleTime;
    [Header("Ñ²Âß")]    
    public float patrolRadius;
    public float patrolSpeed;
    [HideInInspector] public Vector3 spawnPoint;
    [Header("×·»÷")]
    public float maxChaseRadius;
    public float chaseSpeed;
    public bool isOutRange;
    private Collider[] outRangeCls;
    [Header("ÊÜÉË")]
    public bool isHurt;
    public bool isDead;
    public GameObject hurtEffectBlade;
    public GameObject hurtEffectBullet;
    [Header("¼ì²âÍæ¼Ò")] 
    public bool isFindPlayer;
    public float findPlayerRadius;
    public LayerMask playerLayer;  
    public Transform playerTrans;
    private Collider[] findPlayerCls;
    public Vector2 findPlayerOffset;
    [HideInInspector] public float findPlayerOffsetX;
    [Header("µØÃæ¼ì²â")]
    public bool isGround;
    public float groundRadius;
    public LayerMask groundLayer;
    private Collider[] groundCls;
    public Vector2 groundOffset;
    [HideInInspector]public float groundOffsetX;
    [Header("¼ì²â¹¥»÷·¶Î§")]
    public bool isCanAttack;
    public float attackRadius;
    private Collider[] attackCls;
    public Vector2 attackOffset;
    [HideInInspector] public float attackOffsetX;
    protected override void Awake()
    {
        base.Awake();
        stateDic.Add(DroneStateEnum.Idle, new DroneIdle());
        stateDic.Add(DroneStateEnum.Patrol, new DronePatrol());
        stateDic.Add(DroneStateEnum.Chase, new DroneChase());
        stateDic.Add(DroneStateEnum.Attack, new DroneAttack());
        stateDic.Add(DroneStateEnum.Hurt, new DroneHurt());
        stateDic.Add(DroneStateEnum.Dead, new DroneDead());

        spawnPoint=this.transform.position;
        groundOffsetX = Mathf.Abs(groundOffset.x);
        findPlayerOffsetX = Mathf.Abs(findPlayerOffset.x);
        attackOffsetX = Mathf.Abs(attackOffset.x);
    }

    private bool isRuning;
    private void OnEnable()
    {
        SwitchState(DroneStateEnum.Idle);
        isRuning = true;
    }

    private void Update()
    {
        this.Check();
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

    private void CalculateOffset()
    {
        groundOffset = new Vector2(groundOffsetX * this.transform.localScale.x, groundOffset.y);
        findPlayerOffset = new Vector2(findPlayerOffsetX * -this.transform.localScale.x, findPlayerOffset.y);
        attackOffset = new Vector2(attackOffsetX * -this.transform.localScale.x, attackOffset.y);
    }
    private void Check()
    {
        CalculateOffset();
        #region ¼ì²âÍæ¼Ò
        findPlayerCls = Physics.OverlapSphere(this.transform.position + (Vector3)findPlayerOffset, findPlayerRadius,playerLayer);
        if (findPlayerCls.Length > 0)
            isFindPlayer = true;
        else
            isFindPlayer = false;
        if (isFindPlayer)
        {
            foreach(Collider collider in findPlayerCls)
            {
                if (collider.CompareTag("Player"))
                    playerTrans = collider.gameObject.transform;
            }
        }
        #endregion
        #region ¼ì²â¹¥»÷·¶Î§
        attackCls= Physics.OverlapSphere(this.transform.position+(Vector3)attackOffset, attackRadius, playerLayer);
        if(attackCls.Length>0)
            isCanAttack = true;
        else
            isCanAttack= false;
        #endregion
        #region µØÃæ¼ì²â
        Collider[] cls = Physics.OverlapSphere(this.transform.position + (Vector3)groundOffset, groundRadius, groundLayer);
        if(cls.Length>0)
            isGround= true;
        else
            isGround= false;

        #endregion
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
        isHurt = true;
        Vector2 dir = new Vector2(this.transform.position.x - attackerTrans.position.x, 0).normalized;
        this.rb.velocity = dir * hurtForce;
    }
    private IEnumerator HurtEffect()
    {
        yield return new WaitForSeconds(0.2f);
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
    public Vector3 GetNewPoint()
    {
        float targetX = Random.Range(-patrolRadius,patrolRadius);
        float targetY = Random.Range(-patrolRadius,patrolRadius);
        return spawnPoint+new Vector3(targetX,targetY);
    }

    protected override void StopTimeDo()
    {
        beStop = true;
        if (!isDead)
            animator.speed = 0;
    }
    public void SwitchState(DroneStateEnum droneStateNum)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = stateDic[droneStateNum];
        currentState.OnEnter(this);
    }
    public void FlipTo(Vector3 targetPos)
    {
        if (targetPos != null)
        {
            float dir = targetPos.x - this.transform.position.x;
            this.transform.localScale = new Vector3(dir > 0 ? -1 : 1, 1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(this.transform.position + (Vector3)findPlayerOffset, findPlayerRadius);
        //Ñ²Âß·¶Î§
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, patrolRadius);
        if(!isRuning)
            Gizmos.DrawWireSphere(this.transform.position, patrolRadius);
        //×î´ó×·»÷·¶Î§
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(spawnPoint, maxChaseRadius);
        if (!isRuning)
            Gizmos.DrawWireSphere(this.transform.position, maxChaseRadius);
        //¿ÉÍ£ÏÂ¹¥»÷µÄ·¶Î§
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)attackOffset, attackRadius);
        //Ê¶±ðµØÃæµÄ·¶Î§
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)groundOffset, groundRadius);
    }
}

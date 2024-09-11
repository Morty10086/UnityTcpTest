using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using SpidertanTask;
using System.Collections;

public class SpidertankTree : BehaviorTree.Tree
{
    //组件
    [HideInInspector]public Animator animator;
    [HideInInspector]public Rigidbody rb;
    [HideInInspector] public LineRenderer lr;
    public Transform playerTrans;
    [Header("受击特效")]
    public GameObject hurtEffectBlade;
    public GameObject hurtEffectBullet;
    [Header("受击音效")]
    public AudioID bladeAudioID;
    public AudioID bulletAudioID;
    [Header("追击移动")]
    public float waitTime0;
    public float chaseSpeed;
    [Header("平A")]
    public bool isCanAttack;
    public float attackRange;
    public LayerMask attackLayer;
    public Vector2 attackOffset;
    public float attackOffsetX;
    [Header("SkillOne")]
    public Vector3 startPos;
    public Vector3 targetPos;
    public float aimTime;
    public float jumpSpeed;
    public float jumpMaxY;
    public float a;
    public float jumpTime;
    public GameObject skillOneWarnning;

    public float landCheckRange;
    public Vector2 landOffset;
    public float landOffsetX;
    public LayerMask landLayer;

    public float landPosOffX;
    [Header("SkillTwo")]
    public Transform[] skillTwoEdges;
    public float rocketMoveSpeed;
    [Header("ChangeStage")]
    public Transform[] changeStageTargets;
    public GameObject laserWarnnig;
    public GameObject laserCharge;
    public GameObject fire;
    [Header("Stage2")]
    public int index = 0;
    public GameObject stage2Skill1Warnning;
    public GameObject stage2LaserWarnnig;
    public Transform centerPoint;
    public float waitTime;
    [Header("Dead")]
    public bool isDead;

    private void Awake()
    {
        animator = this.GetComponentInChildren<Animator>();
        rb = this.GetComponentInChildren<Rigidbody>();
        attackOffsetX = Mathf.Abs(attackOffset.x);
        landOffsetX = Mathf.Abs(landOffset.x);
    }
    protected override void Start()
    {
        base.Start();     
    }
    protected override void Update()
    {
        base.Update();
        this.transform.position =new Vector3(this.transform.position.x, this.transform.position.y, 35f);
    }
    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            new DeadTask(this.transform),
            new Sequence(new List<Node>()
            {
                new CheckHpTask(this.transform),
                new HalfHpLaserTask(this.transform),
            }),
            new Sequence(new List<Node>()
            {
                new CheckStageTask(this.transform),
                new Selector(new List<Node>()
                {
                    new LongRangeAttackTask(this.transform),
                    new HalfHpRocketTask(this.transform),
                })
            }),
            new Sequence(new List<Node>()
            {
                new CheckAttackRangeTask(this.transform),
                new AttackTask(this.transform),
                new Selector(new List<Node>()
                {
                    new Sequence(new List<Node>()
                    {
                        new SkillOneJumpTask(this.transform),
                        new SkillOneLandTask(this.transform),
                    }),
                    new SkillTwoTask(this.transform),
                }),
            }),
            new ChaseTask(playerTrans,this.transform),
        });
        return root;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)landOffset, landCheckRange);
    }

    public void FlipTo(Transform tagetTrans)
    {
        if (tagetTrans != null)
        {
            float dir = tagetTrans.position.x - this.transform.position.x;
            this.transform.localScale = new Vector3(dir > 0 ? -1 : 1, 1, 1);
        }
    }

    public void GetHurt(Transform attackTrans,bool attackType=false)
    {
        if (attackType)
        {
            hurtEffectBullet.SetActive(true);
            AudioMgr.Instance.PlaySoundNew(AudioID.mHurtBullet);
        }
        else
        {
            hurtEffectBlade.SetActive(true);
            AudioMgr.Instance.PlaySoundNew(AudioID.mHurtBlade);
        }            
        StartCoroutine(HurtEffect());
    }
    private IEnumerator HurtEffect()
    {
        yield return new WaitForSeconds(0.2f);
        hurtEffectBlade.SetActive(false);
        hurtEffectBullet.SetActive(false);
    }
    public void GetDead()
    {
        isDead = true;
    }

    public void Die()
    {
        
    }
}

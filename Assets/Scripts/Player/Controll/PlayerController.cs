using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int PlayerID;
    public PlayerMsg playerMsg=new PlayerMsg();
    public PlayerMsg playerMsgOP = new PlayerMsg();
    public float velocityX;
    public float velocityY;
    public PlayerCharacter pCharacter;
    public bool isP1;

    private bool hasEnableST;
    public Vector2 nowSpeed; 
    [HideInInspector]public PlayerControllerInput playerInput;
    #region 物体组件
    public Rigidbody rb;
    public PhysicsCheck phyCheck;
    private PlayerAnimation pAnimation;
    #endregion
    [Header("移动相关")]
    public float currentSpeed;
    public float runSpeed;
    public Vector2 moveDirection;
    private Vector3 rightDirection=new Vector3(1,1,1);
    private Vector3 leftDirection=new Vector3(-1, 1, 1);
    bool isMaxSpeed;
    float time = 0;
    [Header("跳跃相关")]
    public bool isJump;
    public float jumpForce;
    public bool isCanJump2;
    public int maxJupmCount;
    public int currentJumpCount;

    [Header("攻击相关")]
    //v01
    public bool isAttack;
    private Vector3 targetPos;
    //攻击蹭步
    public float attackForce;

    //v02
    public float normalizedTime;
    public int comboCunter;
    [HideInInspector]
    public List<AttackSO> comboList;
    [HideInInspector]
    float lastInputTime;
    [HideInInspector]
    float lastComboEndTime;   
    [HideInInspector]
    public float time1;
    [HideInInspector]
    public float time2;   
    [HideInInspector]
    public float endComboTime;

    //v03(借用v02中的comboCunter和normalizedTime)
    private AnimatorStateInfo animStateInfo;   
   
    
    [Header("冲刺相关")]
    public float dashDistance;
    public float dashTime;
    public float nowDashTime;
    public bool isDash;   
    public float dashSpeed;
    public GameObject shadowObj;
    public int dashCountMax;
    public int dashCounter;
    public float dashCD;
    public float dashCDCounter;
    [Header("时停相关")]
    public float recoverSpeed;
    public float stopTime;
    public float nowStopTime;
    public float stopTimeNeed;
    public float stopTimeCounter;
    public bool isCounterMax;
    public static bool isStopTime;    
    public bool testIsStopTime;
    //public UnityEvent bulletTimeEnemy;
    //范围时停
    public bool testIsRangeStopTime;
    public static bool isRangeStopTime;
    public  float rangeStopTimeNeed;
    public GameObject stopRangeObj;
    [Header("子弹相关")]
    public Vector2 bulletBeginOffset;
    public float drawBulletPosR;
    public float bulletBeginOffsetX;
    public bool isShoot;
    public bool isReadyShoot;
    public bool canShootEnd;
    //最大射击次数
    public int maxShootCount;
    public int shootCounter;
    [Header("空中攻击")]
    public float jumpAttackForceX;
    public float jumpAttackForceY;
    public int jumpAttackMax;
    public int jumpAttackCounter;
    [Header("空中射击")]
    public float jumpShootForceX;
    public float jumpShootForceY;
    public int jumpShootMax;
    public int jumpShootCounter;
    [Header("交互相关")]
    public static bool isPause;
    //是否可以对话
    public bool canDialogue;
    //是否可以切换场景
    public  bool canChangeScene;
    //目标场景
    public string targetScene;
    public Vector3 playerTargetPos;
    //记录上一个检查点
    public Transform lastCheckPoint;
    public bool isReturnCheckPoint;
    [Header("受伤")]
    public bool isDead;
    public bool isHurt;
    public float hurtForce;
    public GameObject hurtEffectBlade;
    public GameObject hurtEffectBullet;

    private void OnEnable()
    {
        this.playerInput.Enable();
    }
    private void OnDisable()
    {
        this.playerInput.Disable();
    }
    protected virtual void Awake()
    {        
        
        #region 组件获取
        phyCheck = this.GetComponent<PhysicsCheck>();
        rb = this.GetComponent<Rigidbody>();
        pAnimation = this.GetComponent<PlayerAnimation>();
        pCharacter= this.GetComponent<PlayerCharacter>();
        #endregion
        #region 变量初始化
        playerMsg.playerData=new PlayerData();
        playerMsgOP.playerData=new PlayerData();
        playerInput = new PlayerControllerInput();
        Physics.gravity = new Vector3(0,-39.24f,0);
        currentSpeed = runSpeed;
        shootCounter = maxShootCount;
        stopTimeCounter = stopTimeNeed;
        dashCDCounter = dashCD;
        bulletBeginOffsetX=Mathf.Abs(bulletBeginOffset.x);
        #endregion
        #region 按键事件
        playerInput.GamePlay.Jump.started += playerJump;       
        playerInput.GamePlay.Attack.started += playerAttackNew;
        playerInput.GamePlay.Dash.started += playerReadyToDash;
        #endregion
        #region 事件监听
        #endregion
    }



    protected virtual void Update()
    {
        print(NetMgr.Instance.playerID);
        if (NetMgr.Instance.playerID == 1)
        {
            isP1 = true;
        }
        else
        {
            isP1 = false;
        }
        if (isDead)
        {
            isDead = false;
            Cr_LevelManager.Instance.GameOver(!isP1);
            Invoke("ChangeScene", 5f);
            //MySceneManager.Instance.ChangeSceneTo(SceneManager.GetActiveScene().name);
        }

        if (NetMgr.Instance.playerID != this.PlayerID)
        {
            this.playerInput.Disable();

            if (NetMgr.Instance.receivePlayerMsgQueue.Count > 0)
            {
                playerMsgOP=NetMgr.Instance.receivePlayerMsgQueue.Dequeue();
            }
            comboCunter = playerMsgOP.playerData.comboCounter;
            isJump = playerMsgOP.playerData.isJump;
            isAttack = playerMsgOP.playerData.isAttack;
            isHurt = playerMsgOP.playerData.isHurt;
            isDash = playerMsgOP.playerData.isDash;
            isDead = playerMsgOP.playerData.isDead;
            pCharacter.currentHp = playerMsgOP.playerData.currentHp;
            //其他玩家移动
            currentSpeed = runSpeed;
            //if (!isDead && !isHurt && !isAttack && !isDash)
            //{
            //    rb.velocity = new Vector2(playerMsgOP.playerData.velocityX, playerMsg.playerData.velocityY);
            //    print(rb.velocity.x + "_" + rb.velocity.y);
            //}
            this.transform.position = new Vector3(playerMsgOP.playerData.position.X,playerMsgOP.playerData.position.Y, playerMsgOP.playerData.position.Z);
            //其他玩家转向
            if (playerMsgOP.playerData.moveDirectionX > 0)
            {
                this.transform.localScale = rightDirection;
                print("zhuanxiang1");
            }              
            if (playerMsgOP.playerData.moveDirectionX < 0)
            {
                this.transform.localScale = leftDirection;
                print("zhuanxiang-1");
            }

            //其他玩家跳跃
            if (isCanJump2 && phyCheck.isGround && currentJumpCount < maxJupmCount)
            {
                currentJumpCount = maxJupmCount;
            }
            else if (isCanJump2 && !phyCheck.isGround && currentJumpCount > 0)
            {
                currentJumpCount = 1;
            }

            if (isJump)
            {
                if (currentJumpCount > 0)
                    currentJumpCount--;
                isJump = false;
            }
            //其他玩家攻击
            playerAttackInput();
            if (isAttack)
            {
                pAnimation.animator.SetInteger("comboCounter", comboCunter);
            }
            return;
        }
            
        playerTurn();
        if(phyCheck.isGround)
        {
            dashCounter = dashCountMax;
        }
        else if (!phyCheck.isGround && dashCounter > 0)
        {
            dashCounter = 1;
        }
        //获取当前轴输入作为移动方向
        moveDirection = this.playerInput.GamePlay.Move.ReadValue<Vector2>();
        if (moveDirection.x == 0||isAttack||isDash||isHurt||!phyCheck.isGround)
        {
            AudioMgr.Instance.StopSoundNew(AudioID.playerMove);
        }
        //检测二段跳
        if(isCanJump2&&phyCheck.isGround&&currentJumpCount< maxJupmCount)
        {
            currentJumpCount = maxJupmCount;
        }
        else if(isCanJump2 && !phyCheck.isGround&& currentJumpCount>0)
        {
            currentJumpCount = 1;
        }

        //Attack_v03
        playerAttackInput();
        //上传本地数据
        PlayerData playerData=new PlayerData(new System.Numerics.Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
            , moveDirection.x,this.rb.velocity.x,this.rb.velocity.y,comboCunter, isJump, isAttack, isDash, isHurt, isDead,pCharacter.currentHp);
        playerMsg.playerID = NetMgr.Instance.playerID;
        playerMsg.playerData = playerData;
        NetMgr.Instance.GetPlayerMsg(playerMsg);

    }

    protected virtual void FixedUpdate()
    {
        if (!isDead&&!isHurt&&!isAttack && !isDash)
        {
            this.playerMove();
        }
        playerDashNew();    
    }

        //转向
    void playerTurn()
    {
        if (isDash || isHurt || (isAttack && !phyCheck.isGround))
            return;
        if (moveDirection.x > 0)
            this.transform.localScale = rightDirection;
        if (moveDirection.x < 0)
            this.transform.localScale = leftDirection;
               
    }
    protected void playerMove() 
    {
        if (isPause)
            return;
        currentSpeed = runSpeed;
        rb.velocity = new Vector2(moveDirection.x*currentSpeed*Time.deltaTime,rb.velocity.y);
        if (moveDirection.x > 0)
            this.transform.localScale = rightDirection;
        if(moveDirection.x < 0)
            this.transform.localScale = leftDirection;
    }

    //跳跃函数
    private void playerJump(InputAction.CallbackContext context)
    {
        isJump = true;
        if ((phyCheck.isGround||(isCanJump2&&currentJumpCount>0))
            &&!isDash&&!isHurt)
        {           
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            if(currentJumpCount>0)
                currentJumpCount--;
            isJump = false;
        }
            
    }
 
    //v03
    void playerAttackInput()
    {
        animStateInfo = pAnimation.animator.GetCurrentAnimatorStateInfo(1);
        if ((animStateInfo.IsName("playerAttack1")|| 
            animStateInfo.IsName("playerAttack2")||
            animStateInfo.IsName("playerAttack3")||
            animStateInfo.IsName("playerAttack4"))&&
            animStateInfo.normalizedTime>1.0f
            )
        {
            comboCunter = 0;
            pAnimation.animator.SetInteger("comboCounter", comboCunter);
            isAttack = false;
        }
    }
    void playerAttackNew(InputAction.CallbackContext context)
    {
        if (!isHurt&&phyCheck.isGround && !isDash&&!isReadyShoot)
        {            
            if (comboCunter == 0)
            {
                isAttack = true;
                rb.velocity = new Vector3(this.transform.localScale.x, 0, 0) * attackForce;
                comboCunter = 1;
                pAnimation.animator.SetInteger("comboCounter", comboCunter);
            }
            else if (animStateInfo.IsName("playerAttack1") && comboCunter == 1 && animStateInfo.normalizedTime < this.normalizedTime)
            {
                isAttack = true;
                rb.velocity = new Vector3(this.transform.localScale.x, 0, 0) * attackForce;
                comboCunter = 2;
            }
            else if (animStateInfo.IsName("playerAttack2") && comboCunter == 2 && animStateInfo.normalizedTime < this.normalizedTime)
            {
                isAttack = true;
                rb.velocity = new Vector3(this.transform.localScale.x, 0, 0) * attackForce;
                comboCunter = 3;
            }
            else if (animStateInfo.IsName("playerAttack3") && comboCunter == 3 && animStateInfo.normalizedTime < this.normalizedTime)
            {
                isAttack = true;
                rb.velocity = new Vector3(this.transform.localScale.x, 0, 0) * attackForce;
                comboCunter = 4;
            }
        }
        
    }

    //冲刺函数
    //v02
    private void playerReadyToDash(InputAction.CallbackContext context)
    {
        if (isHurt||isPause)
            return;
        if(!isDash&&dashCDCounter>=dashCD&&dashCounter>0)
        {
            //EventCenter.Instance.TriggerEvent("PlaySound", dashClip);
            //AudioMgr.Instance.PlaySound(this.audioSource, dashClip);
            AudioMgr.Instance.PlaySoundNew(AudioID.playerDash);
            isDash = true;
            nowDashTime = 0;
            dashCounter--;
        }
        
    }
    private void playerDashNew()
    {
        if(isDash)
        {
            if(isHurt)
            {
                isDash = false;
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                return;
            }
            if(nowDashTime<dashTime)
            {
                rb.useGravity = false;
                rb.velocity = new Vector2(dashSpeed * this.transform.localScale.x, 0);
                nowDashTime += Time.deltaTime;
                this.gameObject.layer = LayerMask.NameToLayer("Invincible");
                if (nowDashTime >= dashTime)
                {
                    dashCDCounter = 0;
                    isDash =false;
                    rb.velocity = Vector3.zero;
                    rb.useGravity = true;
                    this.gameObject.layer = LayerMask.NameToLayer("Player");
                    canShootEnd = false;
                }
            }
        }
        else
        {
            if(dashCDCounter<dashCD)
                dashCDCounter += Time.deltaTime;
        }
    }
    private IEnumerator TriggerShadow()
    {
        while(isDash)
        {
            yield return new WaitForEndOfFrame();
            PoolManager.Instance.GetObj("Shadow/DashShadow");
        }
    }

    //受伤函数
    public void GetHurt(Transform attackerTrans,bool attackType=false)
    {
        if (isPause)
            return;

        if (attackType)
        {
            hurtEffectBullet.SetActive(true);
            AudioMgr.Instance.PlaySoundNew(AudioID.pHurtBullet);
        }
        else
        {
            hurtEffectBlade.SetActive(true);
            AudioMgr.Instance.PlaySoundNew(AudioID.pHurtBlade);
        }
        StartCoroutine(HurtEffect());
        isHurt = true;
        rb.velocity = Vector2.zero;
        hurtForce = attackerTrans.GetComponentInParent<Attack>().attackForce;
        Vector2 dir = new Vector2((transform.position.x - attackerTrans.position.x), 0).normalized;
        rb.AddForce(dir*hurtForce,ForceMode.Impulse);
    }
    private IEnumerator HurtEffect()
    {
        yield return new WaitForSeconds(0.2f);
        hurtEffectBlade.SetActive(false);
        hurtEffectBullet.SetActive(false);
    }
    //死亡
    public void GetDead()
    {
        this.isDead=true;
        EventCenter.Instance.TriggerEvent("HpChange", this.GetComponent<Character>());
        playerInput.GamePlay.Disable();
        Cr_LevelManager.Instance.GameOver(!isP1);
        Invoke("ChangeScene", 5f);
    }
    public void ChangeScene()
    {
        MySceneManager.Instance.ChangeSceneTo(SceneManager.GetActiveScene().name);
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}

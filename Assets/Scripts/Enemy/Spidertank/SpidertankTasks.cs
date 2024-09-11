using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

namespace SpidertanTask
{
    public class DeadTask:Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;

        public DeadTask() { }
        public DeadTask(Transform spiderTankTrans)
        {
            stTrans = spiderTankTrans;
            stTree = spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }

        public override NodeState Evaluate()
        {
            if (stTree.isDead)
            {
                stTree.fire.SetActive(false);
                stAnimator.Play("DieST");
                info = stAnimator.GetCurrentAnimatorStateInfo(0);
                if (info.IsName("DieST") && info.normalizedTime >= 0.95f)
                {
                    stTree.Die();
                }

                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }
    }
    public class ChaseTask : Node
    {
        private Transform playerTrans;
        private Transform spiderTankTrans;
        private float distance;
        private float moveDir;
        private SpidertankTree stTree;
        private Animator stAnimator;
        public ChaseTask() { }
        public ChaseTask(Transform playerTrans, Transform spiderTankTrans)
        {
            this.playerTrans = playerTrans;
            this.spiderTankTrans = spiderTankTrans;
            stTree=spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            stTree.animator.Play("MoveST");
            distance=playerTrans.position.x-spiderTankTrans.position.x;
            if (Mathf.Abs(distance) > 1)
            {
                spiderTankTrans.localScale = new Vector3(distance > 0 ? -1 : 1, 1, 1);
            }         
            moveDir=-spiderTankTrans.localScale.x;
            this.Move();
            return NodeState.Running;
        }
        private void Move()
        {
            spiderTankTrans.Translate(moveDir*Vector3.right*stTree.chaseSpeed*Time.deltaTime,Space.Self);
        }

    }
    public class CheckAttackRangeTask : Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;

        public CheckAttackRangeTask() { }
        public CheckAttackRangeTask(Transform spiderTankTrans)
        {
            stTree=spiderTankTrans.GetComponent<SpidertankTree> ();
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            stTree.attackOffset = new Vector2(stTree.attackOffsetX * -stTree.transform.localScale.x, stTree.attackOffset.y);
            
            if(GetData("FindPlayer")==null)
            {
                Collider[] cls = Physics.OverlapSphere(stTree.transform.position + (Vector3)stTree.attackOffset, stTree.attackRange, stTree.attackLayer);
                if (cls.Length > 0)
                {
                    parent.AddData("FindPlayer", true);
                    parent.AddData("IsAttack", true);
                    state = NodeState.Success;
                    return state;
                }
                else
                {
                    state=NodeState.Failure;
                    return state;
                }
            }
            else
            {
                state = NodeState.Success;
                return state;
            }

        }
    }
    public class AttackTask : Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;

        private bool hasShake;
        //private float timeCounter;
        public AttackTask() { }
        public AttackTask(Transform spiderTankTrans)
        {
            stTree = spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
            
        }
        public override NodeState Evaluate()
        {
           
            if (GetData("IsAttack")!=null)
            {
                parent.parent.AddData("cantChangeStage", true);
                stAnimator.Play("AttackST");
            }
            info = stAnimator.GetCurrentAnimatorStateInfo(0);    
            if(info.IsName("AttackST") && info.normalizedTime >= 0.4f && !hasShake)
            {
                hasShake = true;
                EventCenter.Instance.TriggerEvent("STAttackShake", null);
            }
            if (info.IsName("AttackST")&&info.normalizedTime >= 0.95f&&GetData("SkillNum")==null)
            {
                ClearData("IsAttack");
                hasShake = false;
                int skillNum = Random.Range(1, 4);             
                parent.AddData("SkillNum", skillNum);
                Debug.Log((int)GetData("SkillNum"));
                parent.AddData("IsSkill", true);
                stTree.startPos = stTree.transform.position;
                stTree.targetPos= stTree.playerTrans.position;
               
            }
            return NodeState.Running;
        }
    }
    public class SkillOneJumpTask:Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private Vector3 startPos;
        private Vector3 targetPos;
        private float aimTimeCounter;
        private bool canJump;
        private bool isAchive;
        private bool isAchiveHalf;
        private float jumpTime;
        private float timeCounter;
        private float v0;
        private float a;
        private float jumpSpeedX;
        private bool canCheckGround;
        private bool landGround;
        public SkillOneJumpTask() { }
        public SkillOneJumpTask(Transform spiderTankTrans)
        {
            stTree=spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            GetTargetPos();

            stTree.landOffset=new Vector3(stTree.landOffsetX*-stTree.transform.localScale.x,stTree.landOffset.y);
            
            if(GetData("SkillNum") != null && (int)GetData("SkillNum") == 3)
            {
                Debug.Log("Not SKill 1");
                state = NodeState.Failure;
                return state;
            }
            if (GetData("IsSkill") != null &&GetData("SkillNum") !=null&&(int)GetData("SkillNum") !=3)
            {
                aimTimeCounter += Time.deltaTime;
                if (aimTimeCounter >= stTree.aimTime)
                {
                    stTree.skillOneWarnning.SetActive(false);
                    if(!canJump)
                        stAnimator.Play("StartJumpST");                    
                }
                else
                {
                    stAnimator.Play("IdleST");
                    stTree.skillOneWarnning.SetActive(true);
                    stTree.skillOneWarnning.transform.position = new Vector3(targetPos.x,3.4f,targetPos.z);
                    Debug.Log("注意，boss即将下砸");
                }                               
            }            
            info = stAnimator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName("StartJumpST") && info.normalizedTime >= 0.95f)
            {
                canJump = true;
            }
            if(canJump)
            {
                stAnimator.Play("JumpST");
                SkillOneJump();
                if(info.IsName("JumpST") && info.normalizedTime >= 0.5f)
                {
                    canCheckGround = true;
                }
                if (info.IsName("JumpST") && info.normalizedTime >= 0.95f && isAchive)
                {

                    EventCenter.Instance.TriggerEvent("STLandShake", null);
                    ClearData("IsSkill");
                    canCheckGround=false;
                    canJump = false;
                    isAchive = false;
                    isAchiveHalf = false;
                    aimTimeCounter = 0;
                    timeCounter = 0;
                    parent.AddData("IsSkillOneLand", true);
                }
            }
           
            state = NodeState.Running;
            return state;
        }

        private void Jump()
        {
            stTree.transform.localScale = new Vector3(targetPos.x - startPos.x > 0 ? -1 : 1, 1, 1);
            Vector3 halfPos = new Vector3((targetPos.x +startPos.x) / 2, stTree.jumpMaxY+startPos.y, stTree.transform.position.z);

            if (!isAchiveHalf)
            {
                stTree.transform.position = Vector3.MoveTowards(stTree.transform.position, halfPos, stTree.jumpSpeed * Time.deltaTime);
            }        
            
            if (Mathf.Abs(stTree.transform.position.x-halfPos.x) < 0.1f&& Mathf.Abs(stTree.transform.position.y - halfPos.y) < 0.1f)
            {
                isAchiveHalf = true;
            }

            if (isAchiveHalf)
            {
                stTree.transform.position = Vector3.MoveTowards(stTree.transform.position, targetPos, stTree.jumpSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(targetPos, stTree.transform.position) < 0.1f)
            {
                isAchive = true;
            }
            else
            {
                isAchive = false;
            }
        }

        //private void Jump()
        //{
        //    stTree.transform.localScale = new Vector3((targetPos.x - startPos.x) > 0 ? -1 : 1, 1, 1);
        //    if (!hasV0)
        //    {
        //        stTree.rb.velocity = new Vector3(0, v0, 0);
        //        Debug.Log("shuzhisudu:"+v0);
        //        hasV0 = true;
        //    }
        //    if (Mathf.Abs(stTree.transform.position.x - targetPos.x) < 0.5f)
        //    {
        //        Debug.Log("到达");
        //        stTree.rb.velocity = new Vector3(0, stTree.rb.velocity.y, 0);
        //        isAchive = true;
        //    }
        //    else
        //    {
        //        Debug.Log("水平移动");
        //        isAchive = false;
        //        stTree.rb.velocity = new Vector3(stTree.jumpSpeed * -stTree.transform.localScale.x, stTree.rb.velocity.y, 0);
        //    }

        //}

        private void GetTargetPos()
        {
            startPos = stTree.startPos;
            if (stTree.targetPos.x > stTree.startPos.x)
            {
                targetPos = new Vector3(stTree.targetPos.x-stTree.landPosOffX, stTree.targetPos.y, stTree.targetPos.z);
            }
            else
            {
                targetPos = new Vector3(stTree.targetPos.x + stTree.landPosOffX, stTree.targetPos.y, stTree.targetPos.z);
            }
        }
        private void SkillOneJump()
        {
            //Debug.Log("cunchushuju");
            //jumpTime = (float)(Mathf.Abs(targetPos.x - startPos.x) / stTree.jumpSpeed);
            //Debug.Log("jumpTime" + jumpTime + Mathf.Abs(targetPos.x - startPos.x));
            //v0 = 4 * stTree.jumpMaxY / jumpTime;
            //Debug.Log("vo" + v0);
            //a = -8 *stTree.jumpMaxY/ (jumpTime * jumpTime);
            //Debug.Log("a" + a);

            jumpSpeedX = (float)(Mathf.Abs(targetPos.x - startPos.x) / stTree.jumpTime);
            v0 = 0.5f * stTree.a * stTree.jumpTime;

            stTree.transform.localScale = new Vector3(stTree.targetPos.x - startPos.x > 0 ? -1 : 1, 1, 1);

            timeCounter += Time.deltaTime;
            float x=jumpSpeedX*-stTree.transform.localScale.x*timeCounter;
            float y=v0*timeCounter-0.5f*stTree.a*timeCounter*timeCounter;
            Collider[] cls = Physics.OverlapSphere(stTree.transform.position + (Vector3)stTree.landOffset, stTree.landCheckRange, stTree.landLayer);
            foreach (Collider c in cls)
            {
                if (c.CompareTag("MovePlatform"))
                {
                    landGround = false;
                    break;
                }
                else
                {
                    landGround=true;
                }
            }
            if((canCheckGround&&cls.Length>0&&landGround)||Mathf.Abs(targetPos.x-stTree.transform.position.x)<0.08f&&Mathf.Abs(targetPos.y - 3.84f) < 0.08f)
            {
                isAchive=true;                
            }
            if(!isAchive)
            {              
                stTree.transform.position=startPos+new Vector3(x,y,0);
            }
        }
    }
    public class SkillOneLandTask : Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private bool isLand;

        private float waitTimeCounter;
        private bool isWait;
        private bool hasShake;
        public SkillOneLandTask() { }
        public SkillOneLandTask(Transform spiderTankTrans)
        {
            stTree = spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }

        public override NodeState Evaluate()
        {
            if (GetData("IsSkillOneLand") != null)
            {
                stAnimator.Play("LandST");
            }
            info = stAnimator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName("LandST") && info.normalizedTime >= 0.95f)
            {
                isLand = true;
                ClearData("IsSkillOneLand");
                stAnimator.Play("LandAttackST");            

            }
            info = stAnimator.GetCurrentAnimatorStateInfo(0);
            if(isLand && info.IsName("LandAttackST") && info.normalizedTime >= 0.4f && !hasShake)
            {
                hasShake = true;
                EventCenter.Instance.TriggerEvent("STAttackShake", null);
            }
            if (isLand&&info.IsName("LandAttackST") && info.normalizedTime >= 0.95f)
            {
                isWait =true;
                waitTimeCounter = 0;
               
            }
            if (isWait)
            {
                stTree.animator.Play("IdleST");
                waitTimeCounter += Time.deltaTime;
                if (waitTimeCounter >= stTree.waitTime0)
                {
                    isWait=false;
                    isLand = false;
                    hasShake=false;
                    ClearData("FindPlayer");
                    ClearData("SkillNum");
                    ClearData("cantChangeStage");
                    Debug.Log("Skill 1 Clear");
                }
            }
            state = NodeState.Running;
            return state;
        }
    }
    public class SkillTwoTask : Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private float moveDir;

        private float waitTimeCounter;
        private bool isWait;
        public SkillTwoTask() { }   
        public SkillTwoTask(Transform spiderTankTrans)
        {
            stTree=spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            Debug.Log("SKill 2");
            if (GetData("IsSkill") != null && GetData("SkillNum") != null && (int)GetData("SkillNum") == 3)
            {
                Debug.Log("Skill Two");
                if (Mathf.Abs(stTree.transform.position.x-stTree.skillTwoEdges[0].position.x)<0.1f
                    || Mathf.Abs(stTree.transform.position.x - stTree.skillTwoEdges[1].position.x) < 0.1f)
                {
                    TurnAndShoot();
                    stAnimator.Play("RocketST");
                    info = stAnimator.GetCurrentAnimatorStateInfo(0);
                    if(info.IsName("RocketST")&&info.normalizedTime>=0.95f)
                    {
                        isWait = true;
                        waitTimeCounter = 0;                       
                    }
                    if (isWait)
                    {
                        stTree.animator.Play("IdleST");
                        waitTimeCounter += Time.deltaTime;
                        if (waitTimeCounter >= stTree.waitTime0)
                        {
                            isWait= false;
                            ClearData("FindPlayer");
                            ClearData("IsSkill");
                            ClearData("SkillNum");
                            ClearData("cantChangeStage");
                        }
                    }
                }
                else
                {
                    stAnimator.Play("MoveST");
                    TurnAndMove();
                }
            }          
            return NodeState.Running;
        }
        private void TurnAndMove()
        {

            if (stTree.startPos.x < stTree.centerPoint.position.x)
            {
                stTree.FlipTo(stTree.skillTwoEdges[0]);
            }
            else
            {
                stTree.FlipTo(stTree.skillTwoEdges[1]);
            }
           moveDir=-stTree.transform.localScale.x;
            stTree.transform.Translate(moveDir * Vector3.right * stTree.rocketMoveSpeed * Time.deltaTime, Space.Self);
        }
        private void TurnAndShoot()
        {
            stTree.FlipTo(stTree.centerPoint);
        }
    }
    public class CheckHpTask : Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;

        private bool hasStartPos;
        public CheckHpTask() { }
        public CheckHpTask(Transform spiderTankTrans)
        {
            stTrans = spiderTankTrans;
            stTree = spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }

        public override NodeState Evaluate()
        {

            if(stTrans.GetComponent<Character>().currentHp<=0.5* stTrans.GetComponent<Character>().maxHp&& GetData("Stage2") == null)
            {
                if (GetData("Laser") == null)
                {
                    parent.AddData("Laser", true);
                }               
                state = NodeState.Success;
                return state;
            }
            else
            {
                state = NodeState.Failure;
                return state;
            }
        }
    }
    public class HalfHpLaserTask : Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;

        private float jumpSpeedX;
        private float jumpTime;
        private float timeCounter;
        private float v0;
        private float a;
        private bool isAchive;
        private bool hasAim;
        private bool canJump;
        private bool hasLand;
        private bool canCheckLand;
        private bool landGround;

        private Vector3 startPos;
        private Vector3 targetPos;
        private bool hasStartPos;

        private bool chargedFinish;
        public HalfHpLaserTask() { }
        public HalfHpLaserTask(Transform spiderTankTrans)
        {
            stTrans = spiderTankTrans;
            stTree=spiderTankTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }

        public override NodeState Evaluate()
        {

            if (GetData("cantChangeStage") != null)
            {
                state = NodeState.Failure;
                return state;
            }

            stTree.landOffset = new Vector3(stTree.landOffsetX * -stTree.transform.localScale.x, stTree.landOffset.y);

            if (GetData("Laser") != null)
            {
                info = stAnimator.GetCurrentAnimatorStateInfo(0);
                if(!isAchive)
                {
                    if (!canJump)
                    {
                        stAnimator.Play("StartJumpST");
                        if(info.IsName("StartJumpST")&&info.normalizedTime>0.95f)
                            canJump = true;
                    }
                    else
                    {
                        stAnimator.Play("JumpST");
                        Jump();
                        if(info.IsName("JumpST") && info.normalizedTime > 0.5f)
                            canCheckLand = true;
                    }                   
                }
                else
                {
                    if(!hasLand)
                        stAnimator.Play("LandST");
                    if(info.IsName("LandST")&&info.normalizedTime>0.95f)
                        hasLand = true;
                    if (hasLand)
                    {
                        stTree.FlipTo(stTree.centerPoint);
                        if (!chargedFinish)
                        {
                            stAnimator.Play("LaserchargedST");
                            stTree.laserWarnnig.SetActive(true);
                            stTree.laserCharge.SetActive(true);
                            if (info.IsName("LaserchargedST") && info.normalizedTime >= 3f)
                            {
                                chargedFinish = true;
                            }
                        }
                        else
                        {
                            stTree.laserWarnnig.SetActive(false);
                            stTree.laserCharge.SetActive(false);
                            stAnimator.Play("LaserST");
                            if (info.IsName("LaserST") && info.normalizedTime >= 0.95f)
                            {
                                parent.parent.AddData("Stage2", true);
                                ClearData("Laser");
                                stTree.fire.SetActive(true);
                                canCheckLand = false;
                                isAchive = false;
                                canJump= false;
                                chargedFinish = false;
                                hasLand = false;                               
                            }
                        }
                    }
                                   
                }
            }

            state= NodeState.Running;
            return state;
        }


        private void Jump()
        {
            if (!hasStartPos)
            {
                hasStartPos = true;
                startPos = stTrans.position;
            }          
            if (startPos.x > stTree.centerPoint.position.x)
            {
                targetPos = stTree.changeStageTargets[0].position;
                stTree.index = 1;
            }
            else
            {
                targetPos = stTree.changeStageTargets[1].position;
                stTree.index = 0;
            }

            jumpSpeedX = (float)(Mathf.Abs(targetPos.x - startPos.x) / stTree.jumpTime);
            v0 = 0.5f * stTree.a * stTree.jumpTime;

            stTree.transform.localScale = new Vector3(targetPos.x - stTree.centerPoint.position.x > 0 ? -1 : 1, 1, 1);

            timeCounter += Time.deltaTime;
            float x = jumpSpeedX * -stTree.transform.localScale.x * timeCounter;
            float y = v0 * timeCounter - 0.5f * stTree.a * timeCounter * timeCounter;
            Collider[] cls = Physics.OverlapSphere(stTree.transform.position + (Vector3)stTree.landOffset, stTree.landCheckRange, stTree.landLayer);
            foreach (Collider c in cls)
            {
                if (c.CompareTag("MovePlatform"))
                {
                    landGround = false;
                    break;
                }
                else
                {
                    landGround = true;
                }
            }
            if ((cls.Length>0&&canCheckLand&&landGround)
                ||(Mathf.Abs(targetPos.x - stTree.transform.position.x) < 0.05f&&Mathf.Abs(targetPos.y - 3.84f) < 0.08f))
            {
                isAchive = true;
            }
            if (!isAchive)
            {
                stTree.transform.position = startPos + new Vector3(x, y, 0);
            }
        }
    }

    public class CheckStageTask : Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private float waitTimeCounter;
        public CheckStageTask() { }
        public CheckStageTask(Transform stTrans)
        {
            this.stTrans = stTrans;
            stTree=stTrans.GetComponent<SpidertankTree>();
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            if (GetData("Stage2") != null)
            {
                if (waitTimeCounter < stTree.waitTime)
                {
                    waitTimeCounter += Time.deltaTime;
                }
                else
                {
                    int stage2SkillNum = Random.Range(1, 3);
                    parent.AddData("Stage2SkillNum", stage2SkillNum);
                }
                if (GetData("Stage2SkillNum") == null)
                {
                    stAnimator.Play("IdleST");
                }
                else
                {
                    waitTimeCounter = 0;
                }
                state = NodeState.Success; 
                return state;
            }
            else
            {
                state = NodeState.Failure;
                return state;
            }
        }
    }
    public class LongRangeAttackTask : Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private float moveDir;

        private float aimTime=1.5f;
        private float aimCounter;

        private Transform targetTrans;
        public LongRangeAttackTask() { }
        public LongRangeAttackTask(Transform stTrans)
        {
            this.stTrans = stTrans;
            stTree=stTrans.GetComponent<SpidertankTree> ();
            stAnimator = stTree .animator;
        }

        public override NodeState Evaluate()
        {
            if (GetData("Stage2SkillNum") != null && (int)GetData("Stage2SkillNum") == 1)
            {
                info = stAnimator.GetCurrentAnimatorStateInfo(0);
                moveDir=-stTrans.localScale.x;

                if (Mathf.Abs(stTrans.position.x - stTree.skillTwoEdges[stTree.index].position.x) < 0.1f)
                {
                    stTree.FlipTo(stTree.centerPoint);
                    if (aimCounter < aimTime)
                    {
                        stAnimator.Play("IdleST");
                        aimCounter += Time.deltaTime;
                        stTree.stage2Skill1Warnning.SetActive(true);
                        stTree.stage2Skill1Warnning.transform.position =new Vector3(stTree.playerTrans.position.x, stTree.playerTrans.position.y+1f, stTree.playerTrans.position.z);
                    }
                    else
                    {
                        if (targetTrans == null)
                        {
                            targetTrans = stTree.playerTrans;
                        }
                        stTree.stage2Skill1Warnning.SetActive(false);
                        stAnimator.Play("Attack2ST");
                        if (info.IsName("Attack2ST") && info.normalizedTime >= 0.95f)
                        {
                            GameObject rocketObj = PoolManager.Instance.GetObj("Bullet/EnemyBullet/STRocket");
                            rocketObj.transform.position=new Vector3(targetTrans.position.x, 25f,targetTrans.position.z);
                            targetTrans = null;
                            aimCounter = 0;
                            ClearData("Stage2SkillNum");
                            stTree.index++;
                            if (stTree.index >= stTree.skillTwoEdges.Length)
                            {
                                stTree.index = 0;
                            }
                        }
                    }
                }
                else
                {
                    TurnAndMove();
                }
                
                state = NodeState.Running;
                return state;
            }
            else
            {
                state = NodeState.Failure;
                return state;
            }
            

        }

        private void TurnAndMove()
        {
            stTree.FlipTo(stTree.skillTwoEdges[stTree.index]);
            stTrans.Translate(stTrans.right * moveDir * stTree.rocketMoveSpeed * Time.deltaTime, Space.World);
            stAnimator.Play("MoveST");
        }
    }
    public class HalfHpSkillTask : Node
    {
        private Transform stTrans;
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;

        private float aimTime = 2;
        private float aimCounter = 0;

        private Transform targetTrans;
        private LineRenderer lr;

        public HalfHpSkillTask() { }
        public HalfHpSkillTask(Transform stTrans)
        {
            this.stTrans = stTrans;
            stTree=stTrans.GetComponent<SpidertankTree>();
            stAnimator=stTree.animator;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("准备技能2");
            if (GetData("Stage2SkillNum") != null && (int)GetData("Stage2SkillNum") == 2)
            {
                Debug.Log("技能2");
                info = stAnimator.GetCurrentAnimatorStateInfo(0);
                if (aimCounter < aimTime)
                {
                    Debug.Log("画线");
                    aimCounter += Time.deltaTime;
                    //画线
                    targetTrans = stTree.playerTrans.transform;
                    stTree.stage2LaserWarnnig.SetActive(true);
                    lr = stTree.stage2LaserWarnnig.GetComponent<LineRenderer>();
                    lr.positionCount = 2;
                    lr.SetPosition(0, stTrans.position);
                    lr.SetPosition(1, targetTrans.position);
                }
                else
                {
                    stTree.stage2LaserWarnnig.SetActive(false);
                }
                state = NodeState.Running;
                return state;
            }
            else
            {
                state = NodeState.Failure;
                return state;
            }

        }
    }
    public class HalfHpRocketTask:Node
    {
        private SpidertankTree stTree;
        private Animator stAnimator;
        private AnimatorStateInfo info;
        private Transform stTrans;
        private float moveDir;

        private Vector3 targetPos;
        private Vector3 startPos;
        private float waitTimeCounter;
        public HalfHpRocketTask() { }
        public HalfHpRocketTask(Transform spiderTankTrans)
        {
            stTree = spiderTankTrans.GetComponent<SpidertankTree>();
            stTrans = spiderTankTrans;
            stAnimator = stTree.animator;
        }
        public override NodeState Evaluate()
        {
            if (GetData("Stage2SkillNum") != null && (int)GetData("Stage2SkillNum") == 2)
            {
                moveDir = -stTrans.localScale.x;
                if (Mathf.Abs(stTrans.position.x - stTree.skillTwoEdges[stTree.index].position.x) < 0.1f)
                {
                    stTree.FlipTo(stTree.centerPoint);
                    stAnimator.Play("RocketST");
                    info = stAnimator.GetCurrentAnimatorStateInfo(0);
                    if (info.IsName("RocketST") && info.normalizedTime >= 0.95f)
                    {
                        ClearData("Stage2SkillNum");
                        stTree.index++;
                        if (stTree.index >= stTree.skillTwoEdges.Length)
                        {
                            stTree.index = 0;
                        }
                    }
                }
                else
                {                    
                    TurnAndMove();
                }
            }
            return NodeState.Running;
        }
        private void TurnAndMove()
        {
            stTree.FlipTo(stTree.skillTwoEdges[stTree.index]);
            stTrans.Translate(stTrans.right * moveDir * stTree.rocketMoveSpeed * Time.deltaTime, Space.World);
            stAnimator.Play("MoveST");
        }
        
    }
}
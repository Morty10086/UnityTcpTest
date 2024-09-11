using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E1GStateEnum 
{
    Idle,
    Walk,
    Run,
    Shoot,
    Hurt,
    Dead
}

public class E1GIdle : BaseState
{
    E1G e1g;
    private float idleTimeCounter;
    public override void OnEnter(Enemy enemy)
    {
        e1g=enemy as E1G;
        this.idleTimeCounter = e1g.idleTime;
        e1g.animator.Play("Gunidle");
        isEnterDone = true;
    }
    public override void LogicUpdate()
    {
        if(e1g.isDead)
        {
            e1g.SwitchState(E1GStateEnum.Dead);
            return;
        }
        if (e1g.beStop)
        {
            return;
        }
        idleTimeCounter -=Time.deltaTime;
        if (e1g.isHurt)
        {
            e1g.SwitchState(E1GStateEnum.Hurt);
        }
        if(e1g.isFindPlayer)
        {
            e1g.SwitchState(E1GStateEnum.Run);
        }
        if (!e1g.isHurt&&idleTimeCounter<=0)
        {
            e1g.SwitchState(E1GStateEnum.Walk);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        idleTimeCounter = 0;
        isEnterDone = false;
    }

    
}

public class E1GWalk:BaseState
{
    E1G e1g;
    private int walkIndex=0;
    public override void OnEnter(Enemy enemy)
    {
        e1g = enemy as E1G;
        e1g.animator.Play("Gunwalk");
        isEnterDone = true;
    }
    public override void LogicUpdate()
    {
        if (e1g.isDead)
        {
            e1g.SwitchState(E1GStateEnum.Dead);
            return;
        }
        if (e1g.beStop)
        {
            return;
        }
        e1g.FlipTo(e1g.walkPoints[walkIndex]);
        this.Walk();
        if(e1g.isHurt)
        {
            e1g.SwitchState(E1GStateEnum.Hurt);
        }
        if (Mathf.Abs(e1g.transform.position.x - e1g.walkPoints[walkIndex].position.x) < 0.1f)
        {
            e1g.SwitchState(E1GStateEnum.Idle);
        }

        if(e1g.isFindPlayer)
        {
            e1g.SwitchState(E1GStateEnum.Run);
        }

    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        isEnterDone = false;
        walkIndex++;
        if (walkIndex >= e1g.walkPoints.Length)
            walkIndex = 0;
    }
    private void Walk()
    {
        e1g.transform.Translate(e1g.transform.right*e1g.moveDir*e1g.walkSpeed*Time.deltaTime,Space.World);
    }
}

public class E1GRun:BaseState
{
    E1G e1g;
    private float runDistance;
    private float deltaDistance;
    public override void OnEnter(Enemy enemy)
    {
        e1g = enemy as E1G;
        runDistance = 0;
        e1g.animator.Play("Gunrun");
        deltaDistance=Mathf.Abs(e1g.transform.position.x-e1g.playerTrans.position.x);
        e1g.runDistance = e1g.safeDistance - deltaDistance;
        isEnterDone = true;
    }
    public override void LogicUpdate()
    {
        if (e1g.isDead)
        {
            e1g.SwitchState(E1GStateEnum.Dead);
            return;
        }
        if (e1g.beStop)
        {
            return;
        }
        e1g.FlipTo(e1g.playerTrans, true);
        this.Run();
        if(e1g.isHurt)
        {
            e1g.SwitchState(E1GStateEnum.Hurt);
        }
        if (runDistance - e1g.runDistance > 0.1f
            ||e1g.transform.position.x < e1g.runPoints[0].position.x
            ||e1g.transform.position.x > e1g.runPoints[1].position.x)
        {
            e1g.SwitchState(E1GStateEnum.Shoot);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        isEnterDone=false;
        runDistance = 0;
    }
    private void Run()
    {
        runDistance += e1g.runSpeed * Time.deltaTime;
        e1g.transform.Translate(e1g.transform.right * e1g.moveDir * e1g.runSpeed * Time.deltaTime, Space.World);
    }
}

public class E1GShoot : BaseState
{
    E1G e1g;
    private AnimatorStateInfo info;
    private float outCombatCounter;
    public override void OnEnter(Enemy enemy)
    {
        e1g = enemy as E1G;
        e1g.animator.Play("Gunattack");
        e1g.FlipTo(e1g.playerTrans);
        isEnterDone = true;
    }
    public override void LogicUpdate()
    {
        if (e1g.isDead)
        {
            e1g.SwitchState(E1GStateEnum.Dead);
            return;
        }
        if (e1g.beStop)
        {
            return;
        }
        if(e1g.isHurt)
        {
            outCombatCounter = 0;
            e1g.SwitchState(E1GStateEnum.Hurt);
        }
        outCombatCounter +=Time.deltaTime;
        if (!e1g.isFindPlayer&&outCombatCounter>=e1g.outCombatTime)
        {
            outCombatCounter=0;
            e1g.SwitchState(E1GStateEnum.Walk);
            return;
        }
        info = e1g.animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>0.95f)
        {
            bool isPlayerOutRange = e1g.playerTrans.position.x < e1g.runPoints[0].position.x || e1g.playerTrans.position.x > e1g.runPoints[1].position.x;
            bool isE1GOutRange= e1g.transform.position.x < e1g.runPoints[0].position.x|| e1g.transform.position.x > e1g.runPoints[1].position.x;
            float distance = Mathf.Abs(e1g.transform.position.x - e1g.playerTrans.position.x);
            
            if ((!isPlayerOutRange&&isE1GOutRange)||distance-e1g.safeDistance>0.1f)
            {
                e1g.SwitchState(E1GStateEnum.Shoot);
            }
            else 
            {
                outCombatCounter = 0;
                e1g.SwitchState(E1GStateEnum.Run);
            }
                    
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        isEnterDone = false;
    }
}

public class E1GHurt : BaseState
{
    E1G e1g;
    private AnimatorStateInfo info;
    public override void OnEnter(Enemy enemy)
    {
        e1g = enemy as E1G;
        e1g.animator.Play("Gunhurt");
    }
    public override void LogicUpdate()
    {
        if(e1g.isDead)
        {
            e1g.SwitchState(E1GStateEnum.Dead);
            return;
        }
        if (e1g.beStop)
        {
            return;
        }
        e1g.playerTrans = GameObject.Find("Player").transform;
        e1g.FlipTo(e1g.playerTrans);
        info = e1g.animator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime>=0.95f)
        {
            e1g.SwitchState(E1GStateEnum.Run);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        e1g.isHurt = false;
    }
}

public class E1GDead : BaseState
{
    E1G e1g;
    public override void OnEnter(Enemy enemy)
    {
        e1g = enemy as E1G;
        e1g.animator.Play("Gundie");
    }
    public override void LogicUpdate()
    {

    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {

    }
}
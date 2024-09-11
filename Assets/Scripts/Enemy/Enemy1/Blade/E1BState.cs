using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using UnityEngine;

public enum E1BStateEnum
{
   Idle,
   Walk,
   Run,   
   Attack,
   Hurt,
   Dead
}
public class E1BIdle : BaseState
{
    E1B e1b;
    private float idleTimeCounter;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;
        this.idleTimeCounter = e1b.idleTime;
        e1b.animator.Play("Bladeidle");
    }
    public override void LogicUpdate()
    {
        if (e1b.isDead)
        {
            e1b.SwitchState(E1BStateEnum.Dead);
            return;
        }
        if (e1b.beStop)
            return;

        idleTimeCounter -= Time.deltaTime;
        if (e1b.isHurt)
        {
            e1b.SwitchState(E1BStateEnum.Hurt);
        }
        if(e1b.isFindPlayer&&!(e1b.transform.position.x < e1b.runPoints[0].position.x
            || e1b.transform.position.x > e1b.runPoints[1].position.x))
        {
            e1b.SwitchState(E1BStateEnum.Run);
        }
        if(!e1b.isHurt&&idleTimeCounter<=0)
        {
            e1b.SwitchState(E1BStateEnum.Walk);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        idleTimeCounter=0;
    }

   
}
public class E1BWalk:BaseState
{
    E1B e1b;
    private int walkIndex=0;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;
        e1b.animator.Play("Bladewalk");


    }
    public override void LogicUpdate()
    {
        if (e1b.isDead)
        {
            e1b.SwitchState(E1BStateEnum.Dead);
            return;
        }
        if (e1b.beStop)
            return;
        e1b.FlipTo(e1b.walkPoints[walkIndex]);
        this.Walk();  
        if (e1b.isHurt)
        {
            e1b.SwitchState(E1BStateEnum.Hurt);
        }
        if (Mathf.Abs(e1b.transform.position.x - e1b.walkPoints[walkIndex].position.x) < 0.1f)
        {
            e1b.SwitchState(E1BStateEnum.Idle);
        }
        if (e1b.isFindPlayer && !(e1b.transform.position.x < e1b.runPoints[0].position.x
            || e1b.transform.position.x > e1b.runPoints[1].position.x))
        {
            e1b.SwitchState(E1BStateEnum.Run);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        walkIndex++;
        if (walkIndex >= e1b.walkPoints.Length)
        {
            walkIndex = 0;
        }
    }

    private void Walk()
    {
        e1b.transform.Translate(e1b.transform.right*e1b.moveDir*e1b.walkSpeed*Time.deltaTime,Space.World);
    }
}

public class E1BRun : BaseState
{
    E1B e1b;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;
        e1b.animator.Play("Bladerun");
    }
    public override void LogicUpdate()
    {
        if (e1b.isDead)
        {
            e1b.SwitchState(E1BStateEnum.Dead);
            return;
        }
        if (e1b.beStop)
            return;

        e1b.FlipTo(e1b.playerTrans);
        this.Run();
        if (e1b.isHurt)
        {
            e1b.SwitchState(E1BStateEnum.Hurt);
        }
        if (e1b.transform.position.x < e1b.runPoints[0].position.x
            ||e1b.transform.position.x> e1b.runPoints[1].position.x)
        {
            //e1b.SwitchState(E1BStateEnum.Idle);
            e1b.SwitchState(E1BStateEnum.Walk);
        }

        if(e1b.isCanAttack)
        {
            e1b.SwitchState(E1BStateEnum.Attack);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {

    }
    private void Run()
    {
        e1b.transform.Translate(e1b.transform.right * e1b.moveDir * e1b.runSpeed * Time.deltaTime, Space.World);
    }
}

public class E1BCut:BaseState
{
    E1B e1b;
    private AnimatorStateInfo info;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;
        e1b.animator.Play("Bladeattack");
    }
    public override void LogicUpdate()
    {
        if (e1b.isDead)
        {
            e1b.SwitchState(E1BStateEnum.Dead);
            return;
        }
        if (e1b.beStop)
            return;

        info = e1b.animator.GetCurrentAnimatorStateInfo(0);       
        if (e1b.isHurt)
        {
            e1b.SwitchState(E1BStateEnum.Hurt);
        }
        if (info.normalizedTime >= 0.95f)
        {
            e1b.SwitchState(E1BStateEnum.Run);         
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {

    }
}

public class E1BHurt:BaseState
{
    E1B e1b;
    private AnimatorStateInfo info;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;       
        e1b.animator.Play("Bladehurt");    
    }
    public override void LogicUpdate()
    {
        if (e1b.isDead)
        {
            e1b.SwitchState(E1BStateEnum.Dead);
            return;
        }
        if (e1b.beStop)
            return;

        e1b.playerTrans = GameObject.FindWithTag("Player").transform;
        e1b.FlipTo(e1b.playerTrans);
        info = e1b.animator.GetCurrentAnimatorStateInfo(0);        
        if (info.normalizedTime >= 0.95f)
        {
            e1b.SwitchState(E1BStateEnum.Run);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        e1b.isHurt=false;
    }
}

public class E1BDead:BaseState
{
    E1B e1b;
    public override void OnEnter(Enemy enemy)
    {
        e1b = enemy as E1B;
        e1b.animator.Play("Bladedie");
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
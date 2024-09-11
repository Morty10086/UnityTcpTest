using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum DroneStateEnum 
{
   Idle,
   Patrol,
   Chase,
   Attack,
   Hurt,
   Dead,
}

public class DroneIdle : BaseState
{
    private Drone drone;
    private float idleTimeCounter;
    public override void OnEnter(Enemy enemy)
    {
        drone=enemy as Drone;
        drone.animator.Play("IdleDrone");
        this.idleTimeCounter = drone.idleTime;
    }
    public override void LogicUpdate()
    {
        
        if (drone.isDead)
        {
            drone.SwitchState(DroneStateEnum.Dead);
            return;
        }
        if (drone.beStop)
            return;
        idleTimeCounter -= Time.deltaTime;
        if (drone.isHurt)
        {
            drone.SwitchState(DroneStateEnum.Hurt);
            return;
        }
        if (drone.isFindPlayer)
        {
            drone.SwitchState(DroneStateEnum.Chase);
            return;
        }
        if (idleTimeCounter <= 0)
        {
            drone.SwitchState(DroneStateEnum.Patrol);
        }
    }
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        
    }

   
}
public class DronePatrol : BaseState
{
    private Drone drone;
    private Vector3 targetPos;
    private AnimatorStateInfo info;
    private bool canMove;
    private Vector2 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        drone = enemy as Drone;
        targetPos=drone.GetNewPoint();
        drone.FlipTo(targetPos);
        drone.animator.Play("StartmoveDrone");
    }
    public override void LogicUpdate()
    {
        if (drone.isDead)
        {
            drone.SwitchState(DroneStateEnum.Dead);
            return;
        }
        if (drone.beStop)
            return;
        if (drone.isHurt)
        {
            drone.SwitchState(DroneStateEnum.Hurt);
            return;
        }
        info = drone.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("StartmoveDrone") && info.normalizedTime >= 0.95f)
        {
            canMove = true;
        }
        if (drone.isFindPlayer)
        {
            drone.SwitchState(DroneStateEnum.Chase);
            return;
        }
        if (canMove)
        {
            drone.animator.Play("MoveDrone");
            this.Move();
        }
        if(Mathf.Abs(targetPos.x-drone.transform.position.x)<0.1f&& Mathf.Abs(targetPos.y - drone.transform.position.y) < 0.1f)
        {
            canMove=false;
            drone.animator.Play("StopmoveDrone");
            if(info.IsName("StopmoveDrone")&&info.normalizedTime>=0.95f)
                drone.SwitchState(DroneStateEnum.Idle);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        canMove = false;
    }

    private void Move()
    {       
        moveDir = (targetPos - drone.transform.position).normalized;
        drone.transform.Translate(moveDir * drone.patrolSpeed * Time.deltaTime, Space.World);
    }
}

public class DroneChase : BaseState
{
    private Drone drone;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector2 moveDir;
    private Vector2 hMoveDir;
    private AnimatorStateInfo info;
    private bool canMove;
    public override void OnEnter(Enemy enemy)
    {
        drone = enemy as Drone;
        startPos = drone.transform.position;
        targetPos=drone.playerTrans.position;
        drone.FlipTo(targetPos);
        drone.animator.Play("StartmoveDrone");
    }
    public override void LogicUpdate()
    {
        if (drone.isDead)
        {
            drone.SwitchState(DroneStateEnum.Dead);
            return;
        }
        if (drone.beStop)
            return;
        if (drone.isHurt)
        {
            drone.SwitchState(DroneStateEnum.Hurt);
            return;
        }
        info = drone.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("StartmoveDrone") && info.normalizedTime >= 0.95f)
        {
            canMove = true;
        }

        if (canMove)
        {
            drone.animator.Play("MoveDrone");
            this.Move();
        }
        if (Mathf.Abs(drone.transform.position.x- drone.spawnPoint.x)>=drone.maxChaseRadius 
            ||Mathf.Abs(drone.transform.position.y - drone.spawnPoint.y)>=drone.maxChaseRadius)
        {
            Debug.Log("³¬³ö·¶Î§");
            canMove = false;
            drone.animator.Play("StopmoveDrone");
            if (info.IsName("StopmoveDrone") && info.normalizedTime >= 0.95f)
                drone.SwitchState(DroneStateEnum.Idle);
            return;
        }

        if (drone.isCanAttack)
        {
            canMove = false;
            drone.SwitchState(DroneStateEnum.Attack);
            //drone.animator.Play("StopmoveDrone");
            //if (info.IsName("StopmoveDrone") && info.normalizedTime >= 0.95f) { }
            //    drone.SwitchState(DroneStateEnum.Attack);
            //return;

        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        canMove=false;
    }
    private void Move()
    {
        moveDir = (targetPos -startPos).normalized;
        if (!drone.isGround)
        {
            drone.transform.Translate(moveDir * drone.chaseSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            hMoveDir = new Vector2(moveDir.x,0).normalized;
            drone.transform.Translate(hMoveDir * drone.chaseSpeed * Time.deltaTime, Space.World);
        }    

    }
}

public class DroneAttack : BaseState
{
    private Drone drone;
    private AnimatorStateInfo info;
    private Vector3 targetPos;
    public override void OnEnter(Enemy enemy)
    {
        drone = enemy as Drone;
        targetPos = drone.playerTrans.position;
        drone.FlipTo(targetPos);
        drone.animator.Play("AttackDrone");
        Debug.Log("Drone¹¥»÷");
    }
    public override void LogicUpdate()
    {
        if (drone.isDead)
        {
            drone.SwitchState(DroneStateEnum.Dead);
            return;
        }
        if (drone.beStop)
            return;
        if (drone.isHurt)
        {
            drone.SwitchState(DroneStateEnum.Hurt);
            return;
        }
        info = drone.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("AttackDrone") && info.normalizedTime >= 0.95f)
        {
            drone.SwitchState(DroneStateEnum.Chase);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {

    }
}

public class DroneHurt : BaseState
{
    private Drone drone;
    private AnimatorStateInfo info;
    public override void OnEnter(Enemy enemy)
    {
        drone = enemy as Drone;
        drone.animator.Play("HurtDrone");
    }
    public override void LogicUpdate()
    {
        if (drone.isDead)
        {
            drone.SwitchState(DroneStateEnum.Dead);
            return;
        }
        if (drone.beStop)
            return;
        info =drone.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("HurtDrone") && info.normalizedTime >= 0.95f)
        {
            drone.SwitchState(DroneStateEnum.Chase);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        drone.isHurt = false;
    }
}

public class DroneDead : BaseState
{
    private Drone drone;
    public override void OnEnter(Enemy enemy)
    {
        drone = enemy as Drone;
        drone.animator.Play("DieDrone");
    }
    public override void LogicUpdate()
    {

    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        drone.isDead = false;
    }
}
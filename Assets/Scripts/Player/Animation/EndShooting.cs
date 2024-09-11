using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndShooting : StateMachineBehaviour
{
    //public float exitShootTime;
    private float time;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponentInParent<PlayerController>().isReadyShoot = false;
        animator.GetComponentInParent<PlayerController>().isShoot = true;
        time = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponentInParent<PlayerController>().isReadyShoot = false;
            animator.GetComponentInParent<PlayerController>().isShoot = false;
            animator.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponentInParent<PlayerController>().isReadyShoot = false;
        animator.GetComponentInParent<PlayerController>().isShoot = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects parent motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

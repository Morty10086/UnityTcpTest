using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndHurtState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Rigidbody>().velocity = Vector2.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (animator.GetComponentInParent<PlayerController>().isReturnCheckPoint)
        //{
        //    animator.GetComponentInParent<PlayerController>().playerReturnCheckPoint();
        //    animator.GetComponentInParent<PlayerController>().isReturnCheckPoint = false;
        //}
        animator.GetComponentInParent<PlayerController>().isHurt=false;
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponentInParent<PlayerController>().isHurt = false;
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

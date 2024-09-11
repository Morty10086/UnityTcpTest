using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //private GameObject playerSpriteRender;
    public Animator animator;
    private Rigidbody rb;
    private PlayerPhysicsCheck phyCheck;
    private PlayerController pController;
    private Character pCharacter;

    private void Awake()

    {
        animator = this.GetComponentInChildren<Animator>();
        rb=this.GetComponent<Rigidbody>();
        phyCheck = this.GetComponentInChildren<PlayerPhysicsCheck>();
        pController = this.GetComponentInChildren<PlayerController>();
        pCharacter= this.GetComponentInChildren<Character>();
    }
    private void Update()
    {
        if (pController.PlayerID!=NetMgr.Instance.playerID)
        {
            if (pController.playerMsgOP != null)
                animator.SetFloat("velocityX",Mathf.Abs(pController.playerMsgOP.playerData.velocityX));
        }
        else
        {
            animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        }
        if (pController.PlayerID != NetMgr.Instance.playerID)
        {
            if (pController.playerMsgOP!=null)
                animator.SetFloat("velocityY", pController.playerMsgOP.playerData.velocityY);
        }
        else 
        {
            animator.SetFloat("velocityY", rb.velocity.y);
        }
      
        animator.SetBool("isGround", phyCheck.isGround);
        animator.SetBool("isAttack", pController.isAttack);
        animator.SetBool("isDash", pController.isDash);
        animator.SetBool("isShoot", pController.isShoot);
        animator.SetBool("canShootEnd", pController.canShootEnd);
        animator.SetBool("isReadyShoot", pController.isReadyShoot);
        animator.SetBool("isHurt", pController.isHurt);
        animator.SetBool("isInvincible", pCharacter.isInvincible);
        animator.SetBool("isDead", pController.isDead);
    }
    public void AttackAnimationTrigger()
    {
        animator.SetTrigger("attack");
    }

    public void ShootAnimationTrigger()
    {
        animator.SetTrigger("Shoot");
    }
    public void JumpAttackAnimationTrigger()
    {
        animator.SetTrigger("jumpAttack");
    }

    public void JumpShootAnimationTrigger()
    {
        animator.SetTrigger("jumpShoot");
    }
}

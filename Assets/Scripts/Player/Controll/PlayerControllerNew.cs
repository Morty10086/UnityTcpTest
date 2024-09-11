using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerNew:PlayerController
{
    public GameObject pSprite;
    private Animator animator;
    protected override void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        animator =pSprite.GetComponent<Animator>();
        playerInput = new PlayerControllerInput();
        currentSpeed = runSpeed;
        playerInput.UI.Interact.started += NewPlayerInteract;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    protected override void Update()
    {
        moveDirection = this.playerInput.GamePlay.Move.ReadValue<Vector2>();
        animator.SetFloat("VelocityX", Mathf.Abs(rb.velocity.x));
    }

    protected override void FixedUpdate()
    {
        this.playerMove();
    }

    private void NewPlayerInteract(InputAction.CallbackContext obj)
    {
        //ÇÐ»»³¡¾°
        if (canChangeScene)
        {
            MySceneManager.Instance.ChangeSceneTo(targetScene, playerTargetPos);
        }
        //¶Ô»°
        if (canDialogue)
        {
            EventCenter.Instance.TriggerEvent("TriggerDialogue", null);
        }
    }


}

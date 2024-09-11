using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformButton : MonoBehaviour
{
    public List<GameObject> platformHList;
    public List<GameObject> platformVList;
    public bool isCanInteract;
    private PlayerControllerInput playerInput;

    public GameObject uiObj;
    private Animator animator;
    private AudioSource audioSource;
    private bool isTrigger;
    private void Awake()
    {
        playerInput=new PlayerControllerInput();
        playerInput.UI.Interact.started += buttonInteract;
        animator = this.GetComponentInChildren<Animator>();
        audioSource = this.GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    private void buttonInteract(InputAction.CallbackContext context)
    {
        if(isCanInteract)
        {
            if(platformHList.Count>0)
            {
                foreach(GameObject platformH in platformHList)
                {
                    platformH.GetComponent<PlatformH>().isCanMove = true;
                }
            }

            if (platformVList.Count > 0)
            {
                foreach (GameObject platformV in platformVList)
                {
                    platformV.GetComponent<PlatformV>().isCanMove = true;
                }
            }
            animator.Play("buttonTrigger");
            uiObj.SetActive(false);
            if (!isTrigger)
            {
                isTrigger = true;
                audioSource.Play();
            }           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isCanInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCanInteract = false;
        }
    }
}

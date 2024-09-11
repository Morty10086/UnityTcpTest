using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Fire : BaseGameLevel
{
    private PlayerController pController;
    public float attackRate;
    private float timeCounter;
    private void OnEnable()
    {
        timeCounter = attackRate;
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeCounter >= attackRate)
            {
                other.GetComponent<Character>().TakeDamage(this);
                timeCounter = 0;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        //pController = collision.gameObject.GetComponent<PlayerController>();
    //        if (timeCounter >= attackRate)
    //        {
    //            collision.gameObject.GetComponent<Character>().TakeDamage(this);
    //            timeCounter = 0;
    //        }
    //        else
    //        {
    //            timeCounter += Time.deltaTime;
    //        }

    //        //pController.playerReturnCheckPoint();
    //    }
    //}

}

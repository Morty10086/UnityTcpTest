using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAttack : Attack
{
    private Pendulum pendulum;
    private void Awake()
    {
        pendulum = this.GetComponentInParent<Pendulum>();
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (!pendulum.isHorizontal && !pendulum.isInRange && !PlayerController.isStopTime && other.CompareTag("Player"))
    //    {
    //        print("Щўзг");
    //        other.GetComponent<Character>().TakeDamage(this);
    //    }
    //}

}

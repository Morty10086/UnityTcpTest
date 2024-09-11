using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : BaseGameLevel
{
    public bool isHorizontal;
    public float speed;
    public float maxRotation;
    public float currentSpeed;
    public Transform target;
    public int rotateDirection;
    public float rotateCounter=0;

   
    private void FixedUpdate()
    {


        if ((isInRange&&PlayerController.isRangeStopTime) || PlayerController.isStopTime)
        {
            StopTimeDo();
        }
        else
        {
            currentSpeed = speed;
            //this.gameObject.GetComponent<Collider>().isTrigger = true;
        }

        if (isHorizontal)
        {
            rotateCounter += currentSpeed * Time.deltaTime*rotateDirection;
            this.transform.RotateAround(target.position, target.forward, currentSpeed * Time.deltaTime * rotateDirection);           
            if(Mathf.Abs(rotateCounter)-maxRotation>0.1f)
            {
                rotateDirection *= -1;
            }
        }
        else
        {
            rotateCounter += currentSpeed * Time.deltaTime * rotateDirection;
            this.transform.RotateAround(target.position, target.right, currentSpeed * rotateDirection * Time.deltaTime);
            if (Mathf.Abs(rotateCounter) >= maxRotation)
            {
                rotateDirection *= -1;
            }
        }

        EndRangeTimeStop();
       
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isHorizontal && !isInRange && !PlayerController.isStopTime && other.CompareTag("Player"))
        {
            other.GetComponent<Character>().TakeDamage(this);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isHorizontal&&!isInRange && !PlayerController.isStopTime && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(this);
        }
    }
    protected override void StopTimeDo()
    {
        this.gameObject.GetComponent<Collider>().isTrigger = false;
        currentSpeed = 0;
    }
    
}

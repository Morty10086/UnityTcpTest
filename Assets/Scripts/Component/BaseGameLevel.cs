using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameLevel : Attack
{
    public GameObject stopTimeRange;
    public bool isInRange;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("StopTime"))
        {
            isInRange = true;
            stopTimeRange = other.gameObject;
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("StopTime"))
        {
            isInRange = true;
            stopTimeRange = other.gameObject;
        }
    }

    protected virtual void EndRangeTimeStop()
    {
        if(stopTimeRange!=null)
        {
            if(!stopTimeRange.activeSelf)
            {
                isInRange=false;
            }
        }
    }
    protected virtual void StopTimeDo()
    {
        
    }
}

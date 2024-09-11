using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StopTimeAbout
{
    public GameObject stopTimeRange
    {
        get;
        set;
    }
    public bool isInRange
    {
        get;
        set;
    }

    protected  void OnTriggerEnter(Collider other);
}

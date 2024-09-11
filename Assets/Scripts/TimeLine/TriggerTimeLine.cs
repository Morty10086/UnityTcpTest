using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerTimeLine : MonoBehaviour
{
    public GameObject timeLineDirector;
    public bool isOnce;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeLineDirector.SetActive(true);
        }
    }
}

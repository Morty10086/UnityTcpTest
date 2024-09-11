using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundChange : MonoBehaviour
{
    public CinemachineVirtualCamera vCamera;
    public GameObject targetBound;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
                vCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = targetBound.GetComponent<Collider>();
         
        }
    }
    
}

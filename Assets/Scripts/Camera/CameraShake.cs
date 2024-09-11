using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource cis;
    private void Awake()
    {
        cis= GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("CameraShake", MakeCameraShake);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("CameraShake", MakeCameraShake);
    }

    private void MakeCameraShake(object info)
    {
        cis.GenerateImpulse();
        print("CamerShake");
    }
}

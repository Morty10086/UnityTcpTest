using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCamerShake : MonoBehaviour
{
    public GameObject attackShakeObj;
    public GameObject landShakeObj;
    private CinemachineImpulseSource attackShake; 
    private CinemachineImpulseSource landShake;

    private void Awake()
    {
        attackShake=attackShakeObj.GetComponent<CinemachineImpulseSource>();
        landShake=landShakeObj.GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("STAttackShake", STAttackShake);
        EventCenter.Instance.AddEventListener("STLandShake", STLandShake);
        
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("STAttackShake", STAttackShake);
        EventCenter.Instance.RemoveEvent("STLandShake", STLandShake);
    }
    private void STAttackShake(object info)
    {
        attackShake.GenerateImpulse();
    }
    private void STLandShake(object info)
    {
        landShake.GenerateImpulse();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeScene : MonoBehaviour
{
    public Vector3 nextScenePos;
    public string netScene;
    private void Awake()
    {
        netScene=this.gameObject.name;     
    }      

}

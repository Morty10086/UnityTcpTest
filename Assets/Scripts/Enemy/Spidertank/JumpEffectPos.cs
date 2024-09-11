using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffectPos : MonoBehaviour
{
    public Transform target;
    private void OnEnable()
    {
        this.transform.parent = null;
        this.transform.position = new Vector3(target.position.x,3.84f,target.position.z);
        Invoke("SetParentAgain", 1f);
    }
    private void OnDisable()
    {
        
    }
    void SetParentAgain()
    {
        this.transform.SetParent(target);
    }
}

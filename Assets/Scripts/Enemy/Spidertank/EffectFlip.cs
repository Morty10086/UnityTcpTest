using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFlip : MonoBehaviour
{
    public bool isLaser;
    public GameObject targetObj;
    public List<GameObject> childernList = new List<GameObject>();

    private void OnEnable()
    {
        if (isLaser&& targetObj.transform.localScale.x<0)
        {
            this.transform.localRotation=Quaternion.Euler(0,180,0);
            return;
        }
        foreach (GameObject obj in childernList)
        {
            obj.transform.localScale = new Vector3(targetObj.transform.localScale.x * Mathf.Abs(obj.transform.localScale.x), obj.transform.localScale.y, obj.transform.localScale.z);
        }
    }
}

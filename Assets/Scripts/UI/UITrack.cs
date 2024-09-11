using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrack : MonoBehaviour
{
    public Transform targetTrans;
    public GameObject[] uiObjs;
    public Vector3 offset;

    private void Awake()
    {

    }
    void FixedUpdate()
    {
        foreach (GameObject ui in uiObjs)
        {
            ui.transform.position = Camera.main.WorldToScreenPoint(targetTrans.position + offset);
        }

    }

}

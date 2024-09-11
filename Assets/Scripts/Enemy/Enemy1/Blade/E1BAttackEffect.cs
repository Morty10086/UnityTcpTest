using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1BAttackEffect : MonoBehaviour
{
    public bool isInstant;
    public List<GameObject> childrenList = new List<GameObject>();
    public E1B e1b;

    private void Awake()
    {
       e1b = GetComponentInParent<E1B>();
    }
    private void OnEnable()
    {
        foreach (GameObject obj in childrenList)
        {
            obj.transform.localScale=new Vector3(Mathf.Abs(obj.transform.localScale.x)* e1b.transform.localScale.x,
                obj.transform.localScale.y, 
                obj.transform.localScale.z);
        }

    }
    private void Update()
    {
        if (!isInstant)
        {
            foreach (GameObject obj in childrenList)
            {
                obj.transform.localScale = new Vector3(Mathf.Abs(obj.transform.localScale.x) * e1b.transform.localScale.x,
                 obj.transform.localScale.y,
                 obj.transform.localScale.z);
            }
        }

    }
}

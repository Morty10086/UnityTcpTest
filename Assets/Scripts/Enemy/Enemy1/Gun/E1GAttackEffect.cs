using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1GAttackEffect : MonoBehaviour
{
    public bool isInstant;
    public List<GameObject> childrenList = new List<GameObject>();
    public E1G e1g;

    private void Awake()
    {
        e1g = GetComponentInParent<E1G>();
    }
    private void OnEnable()
    {
        foreach (GameObject obj in childrenList)
        {
            obj.transform.localScale = new Vector3(Mathf.Abs(obj.transform.localScale.x) * e1g.transform.localScale.x,
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
                obj.transform.localScale = new Vector3(Mathf.Abs(obj.transform.localScale.x) * e1g.transform.localScale.x,
                 obj.transform.localScale.y,
                 obj.transform.localScale.z);
            }
        }

    }
}

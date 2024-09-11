using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShootAffect : MonoBehaviour
{
    public bool isInstant;
    public List<GameObject> childrenList = new List<GameObject>();
    public Drone drone;

    private void Awake()
    {
        drone = GetComponentInParent<Drone>();
    }
    private void OnEnable()
    {
        foreach (GameObject obj in childrenList)
        {
            obj.transform.localScale = new Vector3(-Mathf.Abs(obj.transform.localScale.x) * drone.transform.localScale.x,
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
                obj.transform.localScale = new Vector3(-Mathf.Abs(obj.transform.localScale.x) * drone.transform.localScale.x,
                 obj.transform.localScale.y,
                 obj.transform.localScale.z);
            }
        }

    }
}

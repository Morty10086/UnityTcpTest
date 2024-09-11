
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    public bool isInstant;
   public List<GameObject> childrenList = new List<GameObject>();
   public GameObject player;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        foreach( GameObject obj in childrenList)
        {
            obj.transform.localScale=new Vector3(player.transform.localScale.x*Mathf.Abs(obj.transform.localScale.x),obj.transform.localScale.y,obj.transform.localScale.z);
        }
        
    }
    private void Update()
    {
        if(!isInstant)
        {
            foreach (GameObject obj in childrenList)
            {
                obj.transform.localScale = player.transform.localScale;
            }
        }
        
    }
}

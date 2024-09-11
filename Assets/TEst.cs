using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
    public GameObject player;
    public float time = 0;
    void Start()
    {
        
    }

    
    void Update()
    {
        time += Time.deltaTime;
        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) < 0.1f)
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, this.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 0);
        }
    }
}

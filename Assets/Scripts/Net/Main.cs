using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("NetMgr");
            obj.AddComponent<NetMgr>();
        }
        NetMgr.Instance.ConnectServer("127.0.0.1", 8080);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

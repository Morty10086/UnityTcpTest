using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Link : MonoBehaviour
{
    public InputField input;
    public Button button;
    private void Awake()
    {
        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("NetMgr");
            obj.AddComponent<NetMgr>();
        }
        button.onClick.AddListener(() =>
        {
           bool isSuccess= NetMgr.Instance.ConnectServer(input.text, 8080);
            if (isSuccess)
                this.gameObject.SetActive(false);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITips : MonoBehaviour
{
    public Text currentPlayer;
    
    void Update()
    {
        if (NetMgr.Instance.playerID == 1)
        {
            currentPlayer.text = "��ǰ���:P1";
        }
        else
        {
            currentPlayer.text = "��ǰ���:P2";
        }
    }
}

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
            currentPlayer.text = "当前玩家:P1";
        }
        else
        {
            currentPlayer.text = "当前玩家:P2";
        }
    }
}

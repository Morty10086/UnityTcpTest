using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();

    public bool inStoreTimeLine;
    public bool awakeTimeLine;
    private GameDataMgr() { }
    public static GameDataMgr Instance => instance;
}

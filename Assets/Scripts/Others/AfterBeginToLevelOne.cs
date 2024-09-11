using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBeginToLevelOne : MonoBehaviour
{
    
   public void ToLevelOne()
    {
        MySceneManager.Instance.ChangeSceneTo("LevelOne",default,false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button buttonContinue;
    public Button buttonSettings;
    public Button buttonBack;
   
    public override void Init()
    {
        buttonContinue.onClick.AddListener(() =>
        {
            PlayerController.isPause = false;
            Time.timeScale = 1;
            UIManager.Instance.HidePanel<SettingPanel>();
            if (!GameDataMgr.Instance.inStoreTimeLine)
                GameObject.Find("Player").GetComponent<PlayerController>().playerInput.GamePlay.RangeStopTime.Disable();
        });
        buttonBack.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UIManager.Instance.HidePanel<SettingPanel>();
            MySceneManager.Instance.ChangeSceneTo("BeginScene");
        });
    }
   
    
}

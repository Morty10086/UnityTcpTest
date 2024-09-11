using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button startButton;
    public Button settingButton;
    public Button usButton;
    public Button quitButton;
    public override void Init()
    {
        startButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<BeginPanel>();
            MySceneManager.Instance.ChangeSceneTo("BeginAnimation");          
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

}

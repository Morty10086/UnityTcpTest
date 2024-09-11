using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSettingPanel : MonoBehaviour
{
    public Button openSettingButton;
    public PlayerController playerController;
    public static bool isPause;
    void Start()
    {
        openSettingButton.onClick.AddListener(() =>
        {
            PlayerController.isPause = true;
            UIManager.Instance.ShowPanel<SettingPanel>();
            Time.timeScale = 0;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

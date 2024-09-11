using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isFade;
    private bool isLoadScene;

    //private Vector3 pos=new Vector3(0,0,0);
    private static MySceneManager instance;
    public static MySceneManager Instance=>instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ChangeSceneTo(string sceneTo,Vector3 targetPos=default(Vector3),bool disInput=true)
    {
        if (!isLoadScene)
        {
            isLoadScene = true;
            StartCoroutine(LoadScene(sceneTo, targetPos, disInput));
        }
            
    }

    public void ChangeScene(string sceneTo)
    {
        SceneManager.LoadScene(sceneTo);
    }
    private IEnumerator LoadScene(string sceneTo, Vector3 targetPos = default(Vector3), bool disInput = true)
    {
       
        EventCenter.Instance.ClearEvent();
        PoolManager.Instance.Clear();
        //UIManager.Instance.ClearPanelDic();
        yield return Fade(1);
        AsyncOperation ao=SceneManager.LoadSceneAsync(sceneTo);
        yield return Fade(0);
        isLoadScene = false;
    }

    //黑场淡入TimeLine
    public void TriggerFadeIn()
    {
        StartCoroutine(Fade(1));
    }
    //黑场淡出TimeLine
    public void TriggerFadeOut()
    {
        StartCoroutine(Fade(0));
    }

    //过渡效果
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeTimeLine : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isFade;


    //�ڳ�����TimeLine
    public void TriggerFadeIn()
    {
        StartCoroutine(Fade(1));
    }
    //�ڳ�����TimeLine
    public void TriggerFadeOut()
    {
        StartCoroutine(Fade(0));
    }

    //����Ч��
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

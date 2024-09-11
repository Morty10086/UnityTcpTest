using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour 
{
    
    protected CanvasGroup canvasGroup;

    public float alphaSpeed = 10;
    /// <summary>
    /// ��ǰ�����ػ�����ʾ
    /// </summary>
    public bool isShow = false;
    /// <summary>
    /// ���������Ϻ�ִ�е��߼�
    /// </summary>
    protected UnityAction hideCallBack = null;
    protected virtual void Awake()
    {
        //һ��ʼ�ͻ�ȡ����Ϲ��ص����
        canvasGroup=this.GetComponent<CanvasGroup>();
        //������Ǽ�CanvasGroup���
        if(canvasGroup==null )
        {
            canvasGroup=this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// ע��ؼ��¼�����
    /// ��������嶼��Ҫע��һЩ�ؼ��¼�
    /// ����д�ɳ��󷽷������������ʵ��
    /// </summary>
    public abstract void Init();

    //��ʾ�Լ�
    public virtual  void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }
    //�����Լ�
    public virtual void HideMe(UnityAction callBack)
    {

        isShow=false;
        canvasGroup.alpha=1;
        hideCallBack=callBack;
    }
    protected virtual void Update()
    {
        if(isShow&&canvasGroup.alpha!=1)
        {
            canvasGroup.alpha += alphaSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha >= 1 ) 
            { 
                canvasGroup.alpha = 1;
            }
        }
        else if(!isShow&&canvasGroup.alpha!=0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha<=0)
            {
                canvasGroup.alpha = 0;
                //��嵭����ɺ���ִ�������������Ӧ�߼�
                hideCallBack?.Invoke();
            }
        }
    }
}

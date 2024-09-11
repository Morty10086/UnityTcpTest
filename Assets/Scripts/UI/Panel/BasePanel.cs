using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour 
{
    
    protected CanvasGroup canvasGroup;

    public float alphaSpeed = 10;
    /// <summary>
    /// 当前是隐藏还是显示
    /// </summary>
    public bool isShow = false;
    /// <summary>
    /// 隐藏面板完毕后执行的逻辑
    /// </summary>
    protected UnityAction hideCallBack = null;
    protected virtual void Awake()
    {
        //一开始就获取面板上挂载的组件
        canvasGroup=this.GetComponent<CanvasGroup>();
        //如果忘记加CanvasGroup组件
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
    /// 注册控件事件方法
    /// 所有子面板都需要注册一些控件事件
    /// 所以写成抽象方法，让子类必须实现
    /// </summary>
    public abstract void Init();

    //显示自己
    public virtual  void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }
    //隐藏自己
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
                //面板淡出完成后再执行隐藏面板后的相应逻辑
                hideCallBack?.Invoke();
            }
        }
    }
}

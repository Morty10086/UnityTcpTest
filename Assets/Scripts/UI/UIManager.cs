using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




/// <summary>
/// UI管理器
/// 用于管理各个面板的显示隐藏
/// 提供显示隐藏函数，通过动态加载和删除来显隐
/// </summary>
public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance=>instance;


    //用于存储显示着的面板，每显示一个面板（关闭面板时会从字典中删除）
    private  Dictionary<string,BasePanel> panelDic=new Dictionary<string,BasePanel>();

    //场景中的Canvas对象，用于设置为面板的父对象
    private Transform canvasTrans;
    
    /// <summary>
    /// 构造函数
    /// 调用该单例管理器时自动调用
    /// 加载UI的Canvas和EventSystem用于后续放置面板并且加载
    /// </summary>
    private UIManager()
    {
        //Debug.Log("123");测试代码
        //得到场景中的Canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/UICanvas"));
        canvasTrans = canvas.transform;
        //过场景不移除，保证游戏中只有一个Canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }



    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>()where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //判断字典中是否已经存储了这个面板
        //如果存储了，则直接返回
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        //如果未存储，则根据面板名字动态创建预设体，并将其父对象设置为canvas
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/"+panelName));
        //把此对象放在场景中的Canvas下
        panelObj.transform.SetParent(canvasTrans, false);

        //执行面板上的显示逻辑，并把它保存到字典中
        T panel=panelObj.GetComponent<T>();
        //把这个面板脚本保存到字典中，方便之后的获取和隐藏
        panelDic.Add(panelName, panel);
        //调用自己的显示函数
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否淡出完毕后再删除面板，默认是true</param>
    public void HidePanel<T>(bool isFade=true)where T: BasePanel
    {
        //根据泛型得名字
        string panelName = typeof(T).Name;
        //判断当前显示的面板，有没有想要隐藏的
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里存储的面板脚本
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典里存储的面板脚本
                panelDic.Remove(panelName);
            }
            
        }
       
    }

   //得到面板
   public T GetPanel<T>() where T : BasePanel
    {
        string panelName=nameof(T);
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[(panelName)] as T;
        }
        //没有对应面板（场景上未加载，字典里也就未存储），就返回空
        return null;
    }

    public void ClearPanelDic()
    {
        panelDic.Clear();
    }
}

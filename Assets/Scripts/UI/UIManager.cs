using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




/// <summary>
/// UI������
/// ���ڹ������������ʾ����
/// �ṩ��ʾ���غ�����ͨ����̬���غ�ɾ��������
/// </summary>
public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance=>instance;


    //���ڴ洢��ʾ�ŵ���壬ÿ��ʾһ����壨�ر����ʱ����ֵ���ɾ����
    private  Dictionary<string,BasePanel> panelDic=new Dictionary<string,BasePanel>();

    //�����е�Canvas������������Ϊ���ĸ�����
    private Transform canvasTrans;
    
    /// <summary>
    /// ���캯��
    /// ���øõ���������ʱ�Զ�����
    /// ����UI��Canvas��EventSystem���ں���������岢�Ҽ���
    /// </summary>
    private UIManager()
    {
        //Debug.Log("123");���Դ���
        //�õ������е�Canvas����
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/UICanvas"));
        canvasTrans = canvas.transform;
        //���������Ƴ�����֤��Ϸ��ֻ��һ��Canvas����
        GameObject.DontDestroyOnLoad(canvas);
    }



    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>()where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //�ж��ֵ����Ƿ��Ѿ��洢��������
        //����洢�ˣ���ֱ�ӷ���
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        //���δ�洢�������������ֶ�̬����Ԥ���壬�����丸��������Ϊcanvas
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/"+panelName));
        //�Ѵ˶�����ڳ����е�Canvas��
        panelObj.transform.SetParent(canvasTrans, false);

        //ִ������ϵ���ʾ�߼������������浽�ֵ���
        T panel=panelObj.GetComponent<T>();
        //��������ű����浽�ֵ��У�����֮��Ļ�ȡ������
        panelDic.Add(panelName, panel);
        //�����Լ�����ʾ����
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ񵭳���Ϻ���ɾ����壬Ĭ����true</param>
    public void HidePanel<T>(bool isFade=true)where T: BasePanel
    {
        //���ݷ��͵�����
        string panelName = typeof(T).Name;
        //�жϵ�ǰ��ʾ����壬��û����Ҫ���ص�
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //ɾ������
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //ɾ���ֵ���洢�����ű�
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //ɾ������
                GameObject.Destroy(panelDic[panelName].gameObject);
                //ɾ���ֵ���洢�����ű�
                panelDic.Remove(panelName);
            }
            
        }
       
    }

   //�õ����
   public T GetPanel<T>() where T : BasePanel
    {
        string panelName=nameof(T);
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[(panelName)] as T;
        }
        //û�ж�Ӧ��壨������δ���أ��ֵ���Ҳ��δ�洢�����ͷ��ؿ�
        return null;
    }

    public void ClearPanelDic()
    {
        panelDic.Clear();
    }
}

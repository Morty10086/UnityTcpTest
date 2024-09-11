using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager 
{
    private static PoolManager instance=new PoolManager();
    public static PoolManager Instance=>instance;
    private PoolManager() { }
    public Dictionary<string,List<GameObject>> poolDic=new Dictionary<string,List<GameObject>>();
    private GameObject poolObj;

    public GameObject GetObj(string name)
    {
        GameObject obj=null;
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/"+name));
            obj.name = name;
        }
        obj.SetActive(true);
        obj.transform.parent = null;
        //obj.transform.rotation = Quaternion.identity;
        return obj;
    }

    public void PushObj(string name,GameObject obj)
    {
        if(poolObj==null)
        {
            poolObj=new GameObject("Pool");
            GameObject.DontDestroyOnLoad(poolObj);
        }
        obj.transform.parent=poolObj.transform;
        obj.SetActive(false); 
        if(poolDic.ContainsKey(name))
        {
            poolDic[name].Add(obj);
        }
        else
        {
            poolDic.Add(name,new List<GameObject>() { obj});
        }
    }

    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
    
}

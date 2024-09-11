using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter
{
    private static EventCenter instance = new EventCenter();
    public static EventCenter Instance => instance;
    private EventCenter() { }

    private Dictionary<string,UnityAction<object>> eventDic = new Dictionary<string,UnityAction<object>>();

    public void AddEventListener(string eventName, UnityAction<object> action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            eventDic[eventName] += action;
        }
        else
        {
            eventDic.Add(eventName, action);
        }
    }

    public void TriggerEvent(string eventName,object info)
    {
        if(eventDic.ContainsKey(eventName))
        {
            eventDic[eventName]?.Invoke(info);
        }
    }

    public void RemoveEvent(string eventName,UnityAction<object> action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            eventDic[eventName] -= action;
        }
    }

    public void ClearEvent()
    {
        eventDic.Clear();
    }
}

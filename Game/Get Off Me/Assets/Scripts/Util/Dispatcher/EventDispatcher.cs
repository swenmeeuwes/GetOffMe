using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour, IDispatcher
{
    private Dictionary<string, List<Action<object>>> listeners;

    protected virtual void Start()
    {
        listeners = new Dictionary<string, List<Action<object>>>();
    }

    public void AddEventListener(string type, Action<object> action)
    {
        if (!listeners.ContainsKey(type))
            listeners.Add(type, new List<Action<object>>());

        listeners[type].Add(action);
    }

    public void RemoveEventListener(string type, Action<object> action)
    {
        listeners[type].Remove(action);
    }

    public void Dispatch(string type, object eventObject)
    {
        if (!listeners.ContainsKey(type))
            return;

        var triggers = listeners[type];
        foreach (var trigger in triggers)
        {
            trigger.Invoke(eventObject);
        }
    }
}

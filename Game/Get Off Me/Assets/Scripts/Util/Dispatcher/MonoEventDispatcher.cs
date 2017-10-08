using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEventDispatcher : MonoBehaviour, IDispatcher
{
    private Dictionary<string, List<Tuple<Action<object>, bool>>> listeners;

    protected virtual void Awake()
    {
        listeners = new Dictionary<string, List<Tuple<Action<object>, bool>>>();
    }

    public void AddEventListener(string type, Action<object> action, bool autoClear = false)
    {
        if (!listeners.ContainsKey(type))
            listeners.Add(type, new List<Tuple<Action<object>, bool>>());

        listeners[type].Add(new Tuple<Action<object>, bool>(action, autoClear));
    }

    public void RemoveEventListener(string type, Action<object> action)
    {
        var listenersOfType = listeners[type];
        var foundIndex = -1;
        for (int i = 0; i < listenersOfType.Count; i++)
        {
            var actionItem = listenersOfType[i];
            if(actionItem.item1 == action)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex > -1)
            listenersOfType.RemoveAt(foundIndex);
        else
            Debug.LogWarning("Listener was never added!");
    }

    public void Dispatch(string type, object eventObject)
    {
        if (!listeners.ContainsKey(type))
            return;

        var triggers = listeners[type];
        var awaitingDoom = new List<Tuple<Action<object>, bool>>(); // listeners to be deleted
        foreach (var trigger in triggers)
        {
            trigger.item1.Invoke(eventObject);
            if (trigger.item2)
                awaitingDoom.Add(trigger);
        }

        foreach (var doomedItem in awaitingDoom)
        {
            triggers.Remove(doomedItem);
        }
    }
}

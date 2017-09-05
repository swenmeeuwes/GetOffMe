using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDispatcher {
    void AddEventListener(string type, Action<object> action);
    void RemoveEventListener(string type, Action<object> action);
    void Dispatch(string type, object e);
}

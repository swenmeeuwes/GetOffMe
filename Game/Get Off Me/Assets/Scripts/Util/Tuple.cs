using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tuple<T1, T2> {
    public T1 item1;
    public T2 item2;

    public Tuple(T1 item1, T2 item2)
    {
        this.item1 = item1;
        this.item2 = item2;
    }
}

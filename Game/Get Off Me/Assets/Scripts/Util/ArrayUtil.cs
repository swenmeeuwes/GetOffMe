using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtil {
    public static T GetRandom<T>(T[] array)
    {
        var randomIndex = Mathf.FloorToInt(Random.value * array.Length);
        return array[randomIndex];
    }

    public static T RandomItem<T>(this T[] array)
    {
        return GetRandom<T>(array);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboColorResolver {
    private static readonly int H_VALUE_THRESHOLD = 50; // Value until the max H-value is reached
    private static readonly int S_VALUE_THRESHOLD = 5; // Value until the max H-value is reached

    public static Color Resolve(int combo, float value = 1f)
    {
        float hInterpolation = Mathf.Clamp01((float)combo / H_VALUE_THRESHOLD);
        var hValue = Mathf.Lerp(60f/360f, 1, hInterpolation);

        float sInterpolation = Mathf.Clamp01((float)combo / S_VALUE_THRESHOLD);
        var sValue = Mathf.Lerp(0, 1, sInterpolation);

        return Color.HSVToRGB(hValue, sValue, value);
    }
}

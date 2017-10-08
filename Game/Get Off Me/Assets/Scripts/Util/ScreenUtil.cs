using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenUtil {
    public static bool WorldPositionIsInView(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return screenPosition.x >= 0 && screenPosition.x <= 1 && screenPosition.y >= 0 && screenPosition.y <= 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformUtil {
    public static bool IsAndroid()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
	        return true;
        #else
            return false;
        #endif
    }
}

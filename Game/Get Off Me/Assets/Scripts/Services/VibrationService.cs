﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VibrationService {
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static bool IsEnabled {
        get {
            return PlayerPrefs.GetInt(PlayerPrefsLiterals.DEVICE_VIBRATION.ToString(), 1) == 1;
        }
    }

    public static void Vibrate()
    {
        if (!IsEnabled)
            return;

        if (PlatformUtil.IsAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }


    public static void Vibrate(long milliseconds)
    {
        if (!IsEnabled)
            return;

        if (PlatformUtil.IsAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (!IsEnabled)
            return;

        if (PlatformUtil.IsAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        // Should check is the device has a vibration motor?
        return PlatformUtil.IsAndroid();
    }

    public static void Cancel()
    {
        if (PlatformUtil.IsAndroid())
            vibrator.Call("cancel");
    }
}

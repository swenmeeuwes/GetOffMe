using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AndroidSDKFolder
{
    public static string Path
    {
        get { return EditorPrefs.GetString("AndroidSdkRoot"); }
        set { EditorPrefs.SetString("AndroidSdkRoot", value); }
    }
}
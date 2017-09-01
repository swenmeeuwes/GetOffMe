using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class BuildScript
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "Get Off Me";//PlayerSettings.productName; // PlayerSettings don't work on Jenkins
    static string TARGET_DIR = "Target";

    //[MenuItem("Custom/CI/Build Mac OS X")]
    //static void PerformMacOSXBuild()
    //{
    //    string target_dir = APP_NAME + ".app";
    //    GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXIntel, BuildOptions.None);
    //}

    [MenuItem("Custom/CI/Build Android")]
    static void PerformAndroidBuild()
    {
        string target_dir = APP_NAME + ".apk";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        string res = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);
        if (res.Length > 0) // pls...
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class BuildScript
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "Get Off Me";//PlayerSettings.productName; // PlayerSettings might not work on Jenkins
    static string TARGET_DIR = "Target";

    [MenuItem("Custom/CI/Build Android")]
    static void PerformAndroidBuild()
    {
        string targetFile = APP_NAME + ".apk";
        GenericBuild(SCENES, TARGET_DIR + "/" + targetFile, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None);
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
        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }
}

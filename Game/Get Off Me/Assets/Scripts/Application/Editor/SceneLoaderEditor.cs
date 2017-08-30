using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//[CustomEditor(typeof(SceneLoader))]
//public class SceneLoaderEditor : Editor
//{
//    private SceneLoader sceneLoader;

//    private void OnEnable()
//    {
//        sceneLoader = (SceneLoader)serializedObject.targetObject;
//    }

//    public override void OnInspectorGUI()
//    {        
//        var amountOfScenes = SceneManager.sceneCountInBuildSettings;
//        var availableSceneNames = new string[amountOfScenes];
//        for (int i = 0; i < amountOfScenes; i++)
//        {
//            availableSceneNames[i] = EditorBuildSettings.scenes[i].path;
//        }

//        EditorGUILayout.PrefixLabel("Scene to load");
//        sceneLoader.sceneIndex = EditorGUILayout.Popup(sceneLoader.sceneIndex, availableSceneNames);
//        sceneLoader.sceneName = "t";// SceneManager.GetSceneByBuildIndex(sceneLoader.sceneIndex);
//        //SceneManager.GetSceneByBuildIndex(sceneLoader.sceneIndex).name;
//        // SceneManager.GetSceneByPath(availableSceneNames[sceneLoader.sceneIndex]).name;

//        EditorGUILayout.PrefixLabel("Load delay (s)");
//        sceneLoader.delay = EditorGUILayout.FloatField(sceneLoader.delay);

//        serializedObject.ApplyModifiedProperties();
//    }
//}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyEditor : EditorWindow
{
    private Enemy enemy;
    private string currentFileName;

    private bool showFileNotFoundWarning = false;

    [MenuItem("Window/Enemy Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(EnemyEditor));
        window.minSize = new Vector2(400, 200);
    }

    private void OnEnable()
    {
        enemy = ScriptableObject.CreateInstance<Enemy>();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("File: ");
        currentFileName = GUILayout.TextField(currentFileName, EditorStyles.toolbarTextField, GUILayout.MinWidth(200));
        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            HandleSaveButton();
        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            HandleLoadButton();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        EditorGUILayout.PrefixLabel("Health");
        enemy.health = EditorGUILayout.IntField(enemy.health);

        EditorGUILayout.PrefixLabel("Speed");
        enemy.speed = EditorGUILayout.FloatField(enemy.speed);

        if (showFileNotFoundWarning)
            EditorGUILayout.HelpBox("Could not load enemy, enemy not found", MessageType.Warning);

        GUILayout.EndVertical();
    }

    private void HandleSaveButton()
    {
        AssetDatabase.CreateAsset(enemy, EnemyAssetLocator.Instance.ResolveFileName(currentFileName));
    }

    private void HandleLoadButton()
    {
        var enemyAsset = AssetDatabase.LoadAssetAtPath<Enemy>(EnemyAssetLocator.Instance.ResolveFileName(currentFileName));
        if (enemyAsset != null)
        {
            enemy = enemyAsset;
            showFileNotFoundWarning = false;
        }
        else
        {
            enemy = ScriptableObject.CreateInstance<Enemy>();
            showFileNotFoundWarning = true;
        }
    }
}
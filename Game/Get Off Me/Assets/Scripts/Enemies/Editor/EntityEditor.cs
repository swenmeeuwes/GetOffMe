using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityEditor : EditorWindow
{
    private EntityModel entity;
    private string currentFileName;

    private bool showFileNotFoundWarning = false;

    [MenuItem("Window/Entity Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(EntityEditor));
        window.minSize = new Vector2(400, 300);
    }

    private void OnEnable()
    {
        entity = ScriptableObject.CreateInstance<EntityModel>();
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

        GUILayout.Label("Properties", EditorStyles.boldLabel);

        EditorGUILayout.PrefixLabel("Health");
        entity.health = EditorGUILayout.IntField(entity.health);

        EditorGUILayout.PrefixLabel("Speed");
        entity.speed = EditorGUILayout.FloatField(entity.speed);

        EditorGUILayout.PrefixLabel("Variance in speed");
        entity.varianceInSpeed = EditorGUILayout.FloatField(entity.varianceInSpeed);

        EditorGUILayout.PrefixLabel("Weight");
        entity.weight = EditorGUILayout.FloatField(entity.weight);

        EditorGUILayout.Separator();

        GUILayout.Label("Special", EditorStyles.boldLabel);

        EditorGUILayout.PrefixLabel("Helmet");
        entity.hasHelmet = EditorGUILayout.Toggle(entity.hasHelmet);

        if (showFileNotFoundWarning)
            EditorGUILayout.HelpBox("Could not load enemy, enemy not found", MessageType.Warning);

        GUILayout.EndVertical();
    }

    private void HandleSaveButton()
    {
        AssetDatabase.SaveAssets();
        //AssetDatabase.CreateAsset(entity, EntityAssetLocator.Instance.ResolveFileName(currentFileName));
    }

    private void HandleLoadButton()
    {
        var enemyAsset = AssetDatabase.LoadAssetAtPath<EntityModel>(EntityAssetLocator.Instance.ResolveFileName(currentFileName));
        if (enemyAsset != null)
        {
            entity = enemyAsset;
            showFileNotFoundWarning = false;
        }
        else
        {
            entity = ScriptableObject.CreateInstance<EntityModel>();
            showFileNotFoundWarning = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OffScreenSpawner))]
public class OffScreenSpawnerEditor : Editor
{
    private OffScreenSpawner spawner;
    private void OnEnable()
    {
        spawner = (OffScreenSpawner)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("Developer tools", EditorStyles.boldLabel);

        if(GUILayout.Button("Kill all spawns"))
            spawner.DestroyAllSpawns();

        //serializedObject.Update();
        if(GUI.changed) EditorUtility.SetDirty(spawner);
    }
}

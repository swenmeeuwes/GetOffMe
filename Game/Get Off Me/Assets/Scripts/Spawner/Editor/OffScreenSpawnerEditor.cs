using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OffScreenSpawner))]
public class OffScreenSpawnerEditor : Editor
{
    // TEMP
    GameObject o1;
    int c1;

    GameObject o2;
    int c2;
    //


    private OffScreenSpawner spawner;
    private void OnEnable()
    {
        spawner = (OffScreenSpawner)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // TEMP
        //GUILayout.Label("Spawn list (TEMP)", EditorStyles.boldLabel);

        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label("Prefab");
        //GUILayout.Label("Chance (%)");
        //EditorGUILayout.EndHorizontal();

        //if (spawner.entitySpawnList == null)
        //{
        //    spawner.entitySpawnList = new Tuple<GameObject, int>[] {
        //        new Tuple<GameObject, int>(null, 0),
        //        new Tuple<GameObject, int>(null, 0)
        //    };
        //}

        //EditorGUILayout.BeginHorizontal();
        //spawner.entitySpawnList[0].item1 = (GameObject)EditorGUILayout.ObjectField(spawner.entitySpawnList[0].item1, typeof(GameObject), true);
        //spawner.entitySpawnList[0].item2 = EditorGUILayout.IntField(spawner.entitySpawnList[0].item2);
        //EditorGUILayout.EndHorizontal();

        //EditorGUILayout.BeginHorizontal();
        //spawner.entitySpawnList[1].item1 = (GameObject)EditorGUILayout.ObjectField(spawner.entitySpawnList[1].item1, typeof(GameObject), true);
        //spawner.entitySpawnList[1].item2 = EditorGUILayout.IntField(spawner.entitySpawnList[1].item2);
        //EditorGUILayout.EndHorizontal();

        //spawner.entitySpawnList = new Tuple<GameObject, int>[] {
        //    new Tuple<GameObject, int>(o1, c1),
        //    new Tuple<GameObject, int>(o2, c2)
        //};
        // ---

        GUILayout.Label("Developer tools", EditorStyles.boldLabel);

        if(GUILayout.Button("Kill all spawns"))
            spawner.DestroyAllSpawns();

        //serializedObject.Update();
        if(GUI.changed) EditorUtility.SetDirty(spawner);
    }
}

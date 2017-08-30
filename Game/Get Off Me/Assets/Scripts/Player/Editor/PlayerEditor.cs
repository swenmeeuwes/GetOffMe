using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

    private Player player;

    private void OnEnable()
    {
        player = (Player)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        GUILayout.Label("Editor", EditorStyles.boldLabel);
        player.rateOfSizeIncrease = EditorGUILayout.CurveField(player.rateOfSizeIncrease,GUILayout.Height(100));
       

        serializedObject.ApplyModifiedProperties();
        
    }
}

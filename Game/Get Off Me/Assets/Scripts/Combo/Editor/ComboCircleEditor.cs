using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ComboCircle))]
public class ComboCircleEditor : Editor {
    private ComboCircle comboCircle;
    private void OnEnable()
    {
        comboCircle = (ComboCircle)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("Developer tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Preview circle"))
        {
            comboCircle.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
            comboCircle.GenerateCircle();
        }

        //if (GUILayout.Button("Distort"))
        //    comboCircle.StartDistortLoop();

        //serializedObject.Update();
        if (GUI.changed) EditorUtility.SetDirty(comboCircle);
    }
}

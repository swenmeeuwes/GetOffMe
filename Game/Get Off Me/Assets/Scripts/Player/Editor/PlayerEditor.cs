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

        if (player.sizeInterpolation == null)
            player.sizeInterpolation = new AnimationCurve();
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        EditorGUILayout.PrefixLabel("Health");
        player.health = EditorGUILayout.FloatField(player.health);

        EditorGUILayout.PrefixLabel("Absorb Percentage");
        player.absorbPercentage = EditorGUILayout.IntSlider(player.absorbPercentage, 1, 100);

        EditorGUILayout.PrefixLabel("Size Interpolation");
        player.sizeInterpolation = EditorGUILayout.CurveField(player.sizeInterpolation, GUILayout.Height(100));
       

        serializedObject.ApplyModifiedProperties();
    }
}

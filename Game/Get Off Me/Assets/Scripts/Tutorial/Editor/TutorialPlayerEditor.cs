using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TutorialPlayer))]
public class TutorialPlayerEditor : Editor {
    private TutorialPlayer tutorialPlayer;
    private ReorderableList list;

    private void OnEnable()
    {
        tutorialPlayer = (TutorialPlayer)serializedObject.targetObject;

        list = new ReorderableList(tutorialPlayer.tutorialSequence,
                typeof(List<TutorialItem>),
                true, true, true, true);

        list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Text"), false, () => {
                tutorialPlayer.tutorialSequence.Add(new TutorialText());
            });
            menu.AddItem(new GUIContent("Spawn"), false, () => {
                tutorialPlayer.tutorialSequence.Add(new TutorialSpawn());
            });

            menu.ShowAsContext();
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            var targetObject = element.serializedObject.targetObject;

            if (targetObject is TutorialText)
            {
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Content"), GUIContent.none);
            }


            //var element = list.serializedProperty.GetArrayElementAtIndex(index);
            //rect.y += 2;
            //EditorGUI.PropertyField(
            //    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
            //    element.FindPropertyRelative("Type"), GUIContent.none);
            //EditorGUI.PropertyField(
            //    new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
            //    element.FindPropertyRelative("Prefab"), GUIContent.none);
            //EditorGUI.PropertyField(
            //    new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
            //    element.FindPropertyRelative("Count"), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("Tutorial sequence", EditorStyles.boldLabel);

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

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

        var serializedProperty = serializedObject.FindProperty("tutorialSequence");

        list = new ReorderableList(serializedObject,
                serializedProperty,
                true, true, true, true);

        list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Text"), false, () => {
                tutorialPlayer.tutorialSequence.Add(new TutorialSequenceItem() { type = TutorialSequenceItemType.TEXT });
            });
            menu.AddItem(new GUIContent("Spawn"), false, () => {
                tutorialPlayer.tutorialSequence.Add(new TutorialSequenceItem() { type = TutorialSequenceItemType.SPAWN });
            });

            menu.ShowAsContext();
        };

        list.elementHeightCallback = (int index) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            var elementType = element.FindPropertyRelative("type");

            switch ((TutorialSequenceItemType)elementType.enumValueIndex)
            {
                case TutorialSequenceItemType.TEXT:
                    // TODO
                    break;
                case TutorialSequenceItemType.SPAWN:
                    break;
            }

            // DEFAULT CASE
            return EditorGUIUtility.singleLineHeight;
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            var elementType = element.FindPropertyRelative("type");
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                elementType, GUIContent.none);

            switch((TutorialSequenceItemType)elementType.enumValueIndex)
            {
                case TutorialSequenceItemType.TEXT:
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 60 + 4, rect.y, rect.width - 64, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("textContent"), GUIContent.none);

                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 4, rect.width, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("textDuration"),
                        new GUIContent("Text duration"));

                    break;
                case TutorialSequenceItemType.SPAWN:
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 64, rect.y, rect.width - 64, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("spawnPrefab"), GUIContent.none);
                    break;
            }

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

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

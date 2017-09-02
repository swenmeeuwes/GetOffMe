using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DifficultyManager : EditorWindow {
    public class GamePhase : ScriptableObject
    {
        public int time;
        public float[] percentages;
    }
    [MenuItem("Window/Difficulty Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(DifficultyManager));
        window.minSize = new Vector2(600, 400);
    }
    private List<AbstractEntity> enemyTypes;
    private int enemyTypesCount = 4;

    private List<GamePhase> gamePhases = new List<GamePhase>();

    void OnEnable() {
        handleLoad();
    }
    private void OnGUI() {

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Time");
        for (int i = 0; i < enemyTypesCount; i++) {
            EditorGUILayout.LabelField(" ");
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < gamePhases.Count; i ++) {
            GUILayout.BeginHorizontal();
            gamePhases[i].time = EditorGUILayout.IntField(gamePhases[i].time);
            if (gamePhases[i].percentages == null) {
                gamePhases[i].percentages = new float[enemyTypesCount];
            }
            for (int j = 0; j < gamePhases[i].percentages.Length; j++)
            {
                gamePhases[i].percentages[j] = EditorGUILayout.FloatField(gamePhases[i].percentages[j]);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Phase"))
            addPhase();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save")) { handleSave();  }
            
        GUILayout.EndHorizontal();
    }
    private void addPhase() {
        gamePhases.Add(new GamePhase());
    }
    private void handleSave() {
        for (int i = 0; i < gamePhases.Count; i++)
        {
            AssetDatabase.CreateAsset(gamePhases[i], PhasesAssetLocator.Instance.ResolveFileName("phase_" + i));
        }
    }
    private void handleLoad() {
        string[] aMaterialFiles = Directory.GetFiles(PhasesAssetLocator.Instance.GetSavePath(), "*.asset", SearchOption.AllDirectories);
        foreach (string matFile in aMaterialFiles)
        {
            string assetPath = matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            var sourceMat = AssetDatabase.LoadAssetAtPath<GamePhase>(assetPath);
            
            gamePhases.Add(sourceMat);
        }
    }
}

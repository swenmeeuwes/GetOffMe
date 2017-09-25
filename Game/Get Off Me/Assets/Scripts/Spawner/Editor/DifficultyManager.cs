using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DifficultyManager : EditorWindow {

    [MenuItem("Window/Difficulty Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(DifficultyManager));
        window.minSize = new Vector2(600, 400);
    }

    private List<GamePhase> gamePhases = new List<GamePhase>();
    private EnemyCollection enemyCollection;

    void OnEnable() {
        enemyCollection = ScriptableObject.CreateInstance<EnemyCollection>();
        HandleLoad();
    }
    private void OnGUI() {
        for (int i = 0; i < gamePhases.Count; i++) {
            if (gamePhases[i].weights == null) gamePhases[i].weights = new List<float>();
            if (gamePhases[i].objectKeys == null) gamePhases[i].objectKeys = new List<GameObject>();
        }
        if (enemyCollection.enemyTypes == null) enemyCollection.enemyTypes = new List<GameObject>();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Time");
        for (int i = 0; i < enemyCollection.enemyTypes.Count; i++) {
            EditorGUI.BeginChangeCheck();
            enemyCollection.enemyTypes[i] = (GameObject)EditorGUILayout.ObjectField(enemyCollection.enemyTypes[i], typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (enemyCollection.enemyTypes[i] != null)
                {
                    for (int j = 0; j < gamePhases.Count; j++)
                    {
                        if (gamePhases[j].weights.Count < enemyCollection.enemyTypes.Count) {
                            gamePhases[j].weights.Add(0);
                            gamePhases[j].objectKeys.Add(enemyCollection.enemyTypes[i]);
                        }
                        else{
                            gamePhases[j].objectKeys[i] = enemyCollection.enemyTypes[i];
                        }
                    }
                }
            }
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < gamePhases.Count; i++) {
            GUILayout.BeginHorizontal();

            gamePhases[i].time = EditorGUILayout.IntField(gamePhases[i].time);
            
            for(int j =0; j < gamePhases[i].objectKeys.Count; j++)
            {
                gamePhases[i].weights[j] = EditorGUILayout.FloatField(gamePhases[i].weights[j]);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Phase")) { addPhase(); }
        if (GUILayout.Button("Add Entity")) { addEntity(); }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save")) { HandleSave();  }
        GUILayout.EndHorizontal();
    }
    private void addPhase() {
        GamePhase tmp = new GamePhase();
        tmp.weights = new List<float>();
        tmp.objectKeys = new List<GameObject>();

        for (int i = 0; i < enemyCollection.enemyTypes.Count; i++)
        {
            if (enemyCollection.enemyTypes[i] != null)
            {
                tmp.weights.Add(0);
                tmp.objectKeys.Add(enemyCollection.enemyTypes[i]);
            }
        }
        gamePhases.Add(tmp);
    }
    private void HandleSave() { // werkt niet
        AssetDatabase.SaveAssets();
        for (int i = 0; i < gamePhases.Count; i++)
        {
            AssetDatabase.CreateAsset(gamePhases[i], DifficultyAssetLocator.Instance.ResolveFileNamePhases("phase_" + i));
        }
        AssetDatabase.CreateAsset(enemyCollection, DifficultyAssetLocator.Instance.ResolveFileNameEnemies());
    }
    private void HandleLoad() {
        string[] aMaterialFiles = Directory.GetFiles(DifficultyAssetLocator.Instance.GetPhaseSavePath(), "*.asset", SearchOption.AllDirectories);
        foreach (string matFile in aMaterialFiles)
        {
            string assetPath = matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            GamePhase clone = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GamePhase>(assetPath)) as GamePhase;

            gamePhases.Add(clone);
        }
        enemyCollection = Object.Instantiate(AssetDatabase.LoadAssetAtPath<EnemyCollection>(DifficultyAssetLocator.Instance.ResolveFileNameEnemies())) as EnemyCollection;
        if (enemyCollection == null) {
            enemyCollection = new EnemyCollection();
        }

    }
    private void addEntity() {
        enemyCollection.enemyTypes.Add(null);
    }
}

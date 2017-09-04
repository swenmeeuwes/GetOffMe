using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DifficultyManager : EditorWindow {

    public class EnemyCollection : ScriptableObject {
        public List<GameObject> enemyTypes = new List<GameObject>();
    }

    [MenuItem("Window/Difficulty Editor")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(DifficultyManager));
        window.minSize = new Vector2(600, 400);
    }

    private List<GamePhase> gamePhases = new List<GamePhase>();
    private EnemyCollection enemyCollection;

    void OnEnable() {
        handleLoad();
    }
    private void OnGUI() {
        for (int i = 0; i < gamePhases.Count; i++) {
            if (gamePhases[i].percentages == null) gamePhases[i].percentages = new List<float>();
            if (gamePhases[i].objectKeys == null) gamePhases[i].objectKeys = new List<GameObject>();
        }

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
                        gamePhases[j].percentages.Add(0);
                        gamePhases[j].objectKeys.Add(enemyCollection.enemyTypes[i]);
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
                gamePhases[i].percentages[j] = EditorGUILayout.FloatField(gamePhases[i].percentages[j]);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Phase")) { addPhase(); }
        if (GUILayout.Button("Add Entity")) { addEntity(); }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save")) { handleSave();  }
        GUILayout.EndHorizontal();
    }
    private void addPhase() {
        GamePhase tmp = new GamePhase();
        tmp.percentages = new List<float>();
        tmp.objectKeys = new List<GameObject>();

        for (int i = 0; i < enemyCollection.enemyTypes.Count; i++)
        {
            if (enemyCollection.enemyTypes[i] != null)
            {
                tmp.percentages.Add(0);
                tmp.objectKeys.Add(enemyCollection.enemyTypes[i]);
            }
        }
        gamePhases.Add(tmp);
    }
    private void handleSave() {
        AssetDatabase.SaveAssets();
        for (int i = 0; i < gamePhases.Count; i++)
        {
            AssetDatabase.CreateAsset(gamePhases[i], DifficultyAssetLocator.Instance.ResolveFileNamePhases("phase_" + i));
        }
        AssetDatabase.CreateAsset(enemyCollection, DifficultyAssetLocator.Instance.ResolveFileNameEnemies());
    }
    private void handleLoad() {
        string[] aMaterialFiles = Directory.GetFiles(DifficultyAssetLocator.Instance.GetPhaseSavePath(), "*.asset", SearchOption.AllDirectories);
        foreach (string matFile in aMaterialFiles)
        {
            string assetPath = matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            var sourceMat = AssetDatabase.LoadAssetAtPath<GamePhase>(assetPath);

            gamePhases.Add(sourceMat);
        }
        enemyCollection = AssetDatabase.LoadAssetAtPath<EnemyCollection>(DifficultyAssetLocator.Instance.ResolveFileNameEnemies());
        if (enemyCollection == null) {
            enemyCollection = new EnemyCollection();
        }

    }
    private void addEntity() {
        enemyCollection.enemyTypes.Add(null);
    }
}

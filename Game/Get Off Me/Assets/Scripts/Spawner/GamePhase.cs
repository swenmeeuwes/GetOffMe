using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class GamePhase : ScriptableObject
{
    public int time;
    //public Dictionary<GameObject, float> percentages;
    public List<GameObject> objectKeys;
    public List<float> percentages;

    public static List<GamePhase> loadGamePhasesFromFile() {
        List<GamePhase> gamePhases = new List<GamePhase>();
        string[] aMaterialFiles = Directory.GetFiles(DifficultyAssetLocator.Instance.GetPhaseSavePath(), "*.asset", SearchOption.AllDirectories);
        foreach (string matFile in aMaterialFiles)
        {
            string assetPath = matFile.Replace(Application.dataPath, "").Replace('\\', '/');
            var sourceMat = AssetDatabase.LoadAssetAtPath<GamePhase>(assetPath);

            gamePhases.Add(sourceMat);
        }
        return gamePhases;
    }
}

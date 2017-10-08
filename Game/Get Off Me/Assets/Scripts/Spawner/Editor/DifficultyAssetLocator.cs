using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAssetLocator {
    private readonly string PHASES_SAVE_PATH = "Assets/Resources/Phases";
    private readonly string ENEMY_SAVE_PATH = "Assets/Scripts/Spawner/Editor/Enemies";

    private static DifficultyAssetLocator m_instance;
    public static DifficultyAssetLocator Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new DifficultyAssetLocator();

            return m_instance;
        }
    }
    public string ResolveFileNamePhases(string filename)
    {
        return PHASES_SAVE_PATH + "/" + filename + ".asset";
    }
    public string ResolveFileNameEnemies()
    {
        return ENEMY_SAVE_PATH + "/" + "EnemyCollection" + ".asset";
    }
    public string GetPhaseSavePath() {
        return PHASES_SAVE_PATH;
    }
    public string GetEnemySavePath() {
        return ENEMY_SAVE_PATH + "/" + "EnemyCollection.asset";
    }
}

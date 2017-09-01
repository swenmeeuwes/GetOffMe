using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasesAssetLocator {
    private readonly string ASSET_SAVE_PATH = "Assets/Scripts/Spawner/Editor/Phases";

    private static PhasesAssetLocator m_instance;
    public static PhasesAssetLocator Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new PhasesAssetLocator();

            return m_instance;
        }
    }
    public string ResolveFileName(string filename)
    {
        return ASSET_SAVE_PATH + "/" + filename + ".asset";
    }
    public string GetSavePath() {
        return ASSET_SAVE_PATH;
    }
}

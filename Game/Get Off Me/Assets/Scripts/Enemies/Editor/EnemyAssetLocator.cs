using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssetLocator {
    private readonly string ASSET_SAVE_PATH = "Assets/Scripts/Enemies/Enemy Models";

    private static EnemyAssetLocator m_instance;
    public static EnemyAssetLocator Instance {
        get
        {
            if (m_instance == null)
                m_instance = new EnemyAssetLocator();

            return m_instance;
        }
    }

    public string ResolveFileName(string filename)
    {
        return ASSET_SAVE_PATH + "/" + filename + ".asset";
    }
}

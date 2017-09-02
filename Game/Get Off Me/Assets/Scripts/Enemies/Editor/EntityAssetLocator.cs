using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAssetLocator {
    private readonly string ASSET_SAVE_PATH = "Assets/Scripts/Enemies/Enemy Models";

    private static EntityAssetLocator m_instance;
    public static EntityAssetLocator Instance {
        get
        {
            if (m_instance == null)
                m_instance = new EntityAssetLocator();

            return m_instance;
        }
    }

    public string ResolveFileName(string filename)
    {
        return ASSET_SAVE_PATH + "/" + filename + ".asset";
    }
}

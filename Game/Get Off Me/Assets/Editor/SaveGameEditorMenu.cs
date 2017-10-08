using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveGameEditorMenu : MonoBehaviour {
    [MenuItem("Tools/Save Game/Delete save game")]
    private static void Delete()
    {
        File.Delete(Application.persistentDataPath + "/" + GameManager.SAVEGAME_FILE_NAME);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScripts : MonoBehaviour {
    public void GotoScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

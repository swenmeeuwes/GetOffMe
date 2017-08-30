using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public string sceneName;
    //public int sceneIndex;
    [Tooltip("Time in seconds before the scene gets loaded")]
    public float delay;

	private void Start () {
        Invoke("Execute", delay);
	}

    private void Execute()
    {
        SceneManager.LoadScene(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    [SerializeField]
    private string sceneName;
    [SerializeField][Tooltip("Time in seconds before the scene gets loaded")]
    private float delay;

	private void Start () {
        Invoke("Execute", delay);
	}

    private void Execute()
    {
        SceneManager.LoadScene(sceneName);
    }
}

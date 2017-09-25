using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesScripts : MonoBehaviour {
    public void Start() {
        GetComponent<UnityEngine.UI.Toggle>().isOn = PlayerPrefs.GetInt("ShowTutorial") ==1;
    }

    public void EnableTutorial()
    {
        PlayerPrefs.SetInt("ShowTutorial", 1);
    }
    public void ToggleTutorial() {
        PlayerPrefs.SetInt("ShowTutorial", PlayerPrefs.GetInt("ShowTutorial")==1?0:1);
    }
}

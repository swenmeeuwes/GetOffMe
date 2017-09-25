using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesScripts : MonoBehaviour {
    private Toggle toggle;
    public void Start() {
        toggle = GetComponent<Toggle>();
        
        toggle.isOn = PlayerPrefs.GetInt("ShowTutorial") == 1;
    }

    public void EnableTutorial()
    {
        PlayerPrefs.SetInt("ShowTutorial", 1);
    }

    public void ToggleTutorial() {
        PlayerPrefs.SetInt("ShowTutorial", toggle.isOn ? 1 : 0);
    }
}

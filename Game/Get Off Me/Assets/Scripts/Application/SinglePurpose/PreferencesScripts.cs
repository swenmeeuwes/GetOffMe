using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesScripts : MonoBehaviour {
    private Toggle toggle;
    public void Start() {
        toggle = GetComponent<Toggle>();
        
        toggle.isOn = PlayerPrefs.GetInt(PlayerPrefsLiterals.DID_TUTORIAL) == 0;
    }

    public void EnableTutorial()
    {
        PlayerPrefs.SetInt(PlayerPrefsLiterals.DID_TUTORIAL, 0);
    }

    public void ToggleTutorial() {
        PlayerPrefs.SetInt(PlayerPrefsLiterals.DID_TUTORIAL, toggle.isOn ? 0 : 1);
    }
}

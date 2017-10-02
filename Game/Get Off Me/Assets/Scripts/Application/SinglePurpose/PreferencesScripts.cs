using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesScripts : MonoBehaviour {
    private Toggle toggle;
    public void Start() {
        toggle = GetComponent<Toggle>();
        
        toggle.isOn = PlayerPrefs.GetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString()) == 0;
    }

    public void EnableTutorial()
    {
        PlayerPrefs.SetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), 0);
    }

    [Obsolete]
    public void ToggleTutorial() {
        if(toggle != null)
            PlayerPrefs.SetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), toggle.isOn ? 0 : 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesScripts : MonoBehaviour {
    public void EnableTutorial()
    {
        PlayerPrefs.SetInt("ShowTutorial", 1);
    }
}

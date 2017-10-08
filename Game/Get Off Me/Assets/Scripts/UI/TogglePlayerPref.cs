using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TogglePlayerPref : MonoBehaviour {
    public PlayerPrefsLiterals playerPref;
    public bool defaultValue;

    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.isOn = PlayerPrefs.GetInt(playerPref.ToString(), defaultValue ? 1 : 0) == 1;
    }

    public void Toggle()
    {
        if (toggle != null)
            PlayerPrefs.SetInt(playerPref.ToString(), toggle.isOn ? 1 : 0);
    }
}

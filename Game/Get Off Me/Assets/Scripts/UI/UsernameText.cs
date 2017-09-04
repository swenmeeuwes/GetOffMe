using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UsernameText : MonoBehaviour {
    private Text textField;

    private void Start () {
        textField = GetComponent<Text>();
    }

    private void Update()
    {
        textField.text = GameManager.Instance.PlayerIsAuthenticated ? Social.localUser.userName : "Not logged in";
    }
}

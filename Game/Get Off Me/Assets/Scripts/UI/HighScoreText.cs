﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour {
    private Text textField;

	private void Start () {
        textField = GetComponent<Text>();
    }

    private void Update()
    {
        textField.text = ScoreManager.Instance.Highscore.ToString(); // Listen to score changed event?
    }
}

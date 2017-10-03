﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedVialPanel : MonoBehaviour {

    public static UnlockedVialPanel Instance;
    public Text title;
    public Image image;
    public Text positiveText;
    public Text negativeText;
    public Text description;

    // Use this for initialization
    void Awake() {
        if (Instance != null)
            Debug.LogWarning("Another UnlockedVialPanel was already instantiated!");

        Instance = this;
    }
    void Start() {
        gameObject.SetActive(false);
    }
    public void ShowUnlockedVial(VialData vial) {
        title.text = vial.name;
        image.sprite = vial.sprite;
        positiveText.text = vial.positiveEffect;
        negativeText.text = vial.negativeEffect;
        description.text = vial.description;
        gameObject.SetActive(true);
    }
    public void NextPopup() {
        GameManager.Instance.GameOverNextPanel();
    }
}
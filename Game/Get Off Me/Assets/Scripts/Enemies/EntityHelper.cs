using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHelper : MonoBehaviour {
    [SerializeField]
    private string[] closeCallTexts;

    private ComboSystem comboSystem;

    private void Start()
    {
        comboSystem = FindObjectOfType<ComboSystem>();
    }

    public void ShowCloseCallText()
    {
        comboSystem.ShowEncouragement(closeCallTexts.RandomItem() + "!", true);
        comboSystem.HideEncouragement(2f);
    }
}

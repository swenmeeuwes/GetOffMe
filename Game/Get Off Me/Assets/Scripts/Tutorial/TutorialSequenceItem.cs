using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialSequenceItemType
{
    TEXT,
    SPAWN
}

[Serializable]
public struct TutorialSequenceItem {    
    public TutorialSequenceItemType type;
    public float delay;

    // TEXT
    public string textContent;
    public float textDuration; // In seconds

    // SPAWN
    public GameObject spawnPrefab;
}

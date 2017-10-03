using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialSequenceItemType
{
    TEXT,
    SPAWN,
    COMBO_STATE,
    SHOCKWAVE_CHARGE
}

public enum BinaryEnabledState
{
    ENABLED,
    DISABLED
}

[Serializable]
public struct TutorialSequenceItem {    
    public TutorialSequenceItemType type;
    public float delay;
    public bool waitUntilComplete;

    // TEXT
    public string textContent;
    public float textDuration; // In seconds

    // SPAWN
    public GameObject spawnPrefab;

    // Combo state
    public BinaryEnabledState comboState;

    // Shockwave charge
    public int shockwaveCharge;
}

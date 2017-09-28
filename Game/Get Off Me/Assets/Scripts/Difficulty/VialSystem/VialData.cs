using System;
using UnityEngine;

[Serializable]
public class VialData {
    public VialType type;
    public Sprite sprite;
    public string name;
    public string negativeEffect;
    public string positiveEffect;
    public UnlockConditions unlockConditionType;
    public float unlockConditionValue;
    [Tooltip("Unlock condition text, use {{GOAL}} to show the required value and {{CURRENT}} for the highest achieved value")]
    public string unlockCondition;
}

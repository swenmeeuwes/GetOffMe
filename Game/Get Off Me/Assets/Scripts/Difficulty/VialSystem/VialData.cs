using System;
using UnityEngine;

[Serializable]
public class VialData {
    public VialType type;
    public Sprite sprite;
    public string negativeEffect;
    public string positiveEffect;
    public UnlockConditions unlockConditionType;
    public float unlockConditionValue;
    [Tooltip("Unlock condition text, use {{value}} to show the required value")]
    public string unlockCondition;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty Modifier Database")]
public class DifficultyModifierDatabase : ScriptableObject {
    public DifficultyModifier[] difficultyModifiers;
}

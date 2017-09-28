using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveGameModel {
    public DifficultyModifier[] DifficultyModifiers { get; set; }
    public float TotalTimeAlive { get; set; }
    public int TotalScore { get; set; }
    public int TotalGamesPlayed { get; set; }

    public List<int> EnemyKillCount;
}

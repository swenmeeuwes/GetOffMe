using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScripts : MonoBehaviour {
    public void SetScore(int newScore)
    {
        ScoreManager.Instance.Score = newScore;
    }
}

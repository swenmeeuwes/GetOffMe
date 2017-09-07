using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {
    private GameObject gameOverPanel;

    private bool lastStateEnabled;

    private void Start()
    {
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        lastStateEnabled = false;
    }

    private void Update()
    {
        var newState = GameManager.Instance.State == GameState.GAMEOVER;
        gameOverPanel.SetActive(newState);

        if (!lastStateEnabled && newState)
        {
            Debug.Log(" DEAD");
            Social.ReportScore(ScoreManager.Instance.Score, GooglePlayServiceConstants.leaderboard_score, (bool success) => {
                // handle success or failure
            });
            
        }

        lastStateEnabled = newState;
    }
}

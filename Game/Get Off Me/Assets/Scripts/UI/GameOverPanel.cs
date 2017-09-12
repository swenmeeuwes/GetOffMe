using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {
    [SerializeField]
    private GameObject newPersonalBestPanel;

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

        if (!lastStateEnabled && newState)
        {
            Debug.Log(ScoreManager.Instance.Score + "   " + ScoreManager.Instance.Highscore.ToString());
            var isNewPersonalBest = ScoreManager.Instance.Score >= ScoreManager.Instance.Highscore;
            newPersonalBestPanel.SetActive(isNewPersonalBest);

            ScoreManager.Instance.SubmitHighscore(true);
        }

        gameOverPanel.SetActive(newState);

        lastStateEnabled = newState;
    }
}

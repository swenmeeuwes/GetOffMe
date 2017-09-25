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
            var isNewPersonalBest = ScoreManager.Instance.Score >= ScoreManager.Instance.Highscore;
            newPersonalBestPanel.SetActive(isNewPersonalBest);

            if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1 && ScoreManager.Instance.Score > 100)
                PlayerPrefs.SetInt("ShowTutorial", 0);

            ScoreManager.Instance.SubmitHighscore(true);
        }

        gameOverPanel.SetActive(newState);

        lastStateEnabled = newState;
    }
}

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
        gameOverPanel.SetActive(false);
        lastStateEnabled = false;
    }

    private void Update()
    {
        var newState = GameManager.Instance.State == GameState.GAMEOVER;

        if (!lastStateEnabled && newState)
        {
            var isNewPersonalBest = ScoreManager.Instance.Score >= ScoreManager.Instance.Highscore;
            newPersonalBestPanel.SetActive(isNewPersonalBest);

            if (PlayerPrefs.GetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), 1) == 1 && ScoreManager.Instance.Score > 100)
                PlayerPrefs.SetInt(PlayerPrefsLiterals.SHOW_TUTORIAL.ToString(), 0);

            ScoreManager.Instance.SubmitHighscore(true);

            GameManager.Instance.GameOverSequence(gameOverPanel);
        }

        
        //gameOverPanel.SetActive(newState);

        lastStateEnabled = newState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {
    GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        gameOverPanel.SetActive(GameManager.Instance.State == GameState.GAMEOVER);
    }

    private void Update()
    {
        gameOverPanel.SetActive(GameManager.Instance.State == GameState.GAMEOVER);
    }
}

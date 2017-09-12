using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager {
    private static GameManager _instance;

    public static GameManager Instance {
        get {
            if (_instance == null)
                _instance =  new GameManager();
            return _instance;
        }
    }

    private GameState _state;
    public GameState State {
        get {
            return _state;
        }
        set {
            _state = value;
        }
    }

    public GameManager()
    {
        _state = GameState.MAINMENU;

        if (SceneManager.GetActiveScene().name == "Game")
            Play();
    }
    public void Play() {
        _state = GameState.PLAY;
        Time.timeScale = 1;
    }
    public void Pause() {
        Time.timeScale = 0;
        _state = GameState.PAUSE;
    }
    public void Resume() {
        Time.timeScale = 1;
        _state = GameState.PLAY;
    }
    public void PrepareMainMenu() {
        Time.timeScale = 1;
        _state = GameState.MAINMENU;
    }
}

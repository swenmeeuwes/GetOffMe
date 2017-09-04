using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool PlayerIsAuthenticated; // Google play services

    public GameManager()
    {
        _state = GameState.PLAY;
        PlayerIsAuthenticated = false;
    }
}

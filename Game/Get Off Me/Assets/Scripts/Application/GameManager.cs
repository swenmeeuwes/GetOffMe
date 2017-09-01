using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {
    public GameManager _instance;

    private GameManager Instance {
        get {
            if (_instance == null)
                return new GameManager();
            return _instance;
        }
    }
}

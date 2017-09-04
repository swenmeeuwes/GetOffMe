using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScripts : MonoBehaviour {
    public void SetGameState(string newState)
    {
        SetGameState((GameState)Enum.Parse(typeof(GameState), newState));
    }

    public void SetGameState(GameState newState)
    {
        GameManager.Instance.State = newState;
    }
}

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

    public void SetPause() {
        GameManager.Instance.Pause();
    }

    public void SetResume() {
        GameManager.Instance.Resume();
    }

    public void PrepareMenu() {
        GameManager.Instance.PrepareMainMenu();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ScoreManager
{
    private static ScoreManager _instance;
    public static ScoreManager Instance {
        get {
            if (_instance == null)
                _instance = new ScoreManager();
            return _instance;
        }
    }

    //private Timer timer;

    private ScoreManager() {
        Score = 0;

        //timer = new Timer();
        //timer.Interval = 5000;
        //timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
        //timer.Start();
    }
    public void HandleHighscore() {
        if(Score > PlayerPrefs.GetInt("highscore")) PlayerPrefs.SetInt("highscore", Score);

        Debug.Log("Highscore: "+PlayerPrefs.GetInt("highscore"));
    }

    public int Score { get; set; }

    //private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    //{
    //    Score++;
    //}
}

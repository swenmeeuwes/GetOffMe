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
        Highscore = PlayerPrefs.GetInt("highscore");

        //timer = new Timer();
        //timer.Interval = 5000;
        //timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
        //timer.Start();
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if (_score > Highscore)
            {
                PlayerPrefs.SetInt("highscore", _score);
                Highscore = _score;
            }
        }
    }
    
    public int Highscore { get; private set; }

    //private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    //{
    //    Score++;
    //}
}

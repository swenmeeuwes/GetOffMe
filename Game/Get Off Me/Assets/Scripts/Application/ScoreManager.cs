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
                ActualHighscore = _score;
        }
    }

    private int _actualHighscore;
    public int ActualHighscore
    {
        get
        {
            return _actualHighscore;
        }
        private set
        {
            _actualHighscore = value;
        }
    }

    public int Highscore {
        get
        {
            return PlayerPrefs.GetInt(PlayerPreferenceConstants.Highscore);
        }
        set
        {
            PlayerPrefs.SetInt(PlayerPreferenceConstants.Highscore, value);
            PlayerPrefs.Save();
        }
    }

    public void SubmitHighscore(bool reportToGooglePlay = false)
    {
        Highscore = ActualHighscore;

        if (reportToGooglePlay)
            GooglePlayServicesManager.Instance.ReportScoreToLeaderboard(GooglePlayServiceConstants.leaderboard_score, Highscore);
    }

    //private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    //{
    //    Score++;
    //}
}

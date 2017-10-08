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

    private ScoreManager() {
        Score = 0;
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
            if (_score > ActualHighscore)
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
            return PlayerPrefs.GetInt(PlayerPrefsLiterals.HIGHSCORE.ToString(), 0);
        }
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsLiterals.HIGHSCORE.ToString(), value);
        }
    }

    public void SubmitHighscore(bool reportToGooglePlay = false)
    {
        if(ActualHighscore > Highscore)
            Highscore = ActualHighscore;

        if (reportToGooglePlay)
            GooglePlayServicesManager.Instance.ReportScoreToLeaderboard(GooglePlayServiceConstants.leaderboard_score, Highscore);
    }
}

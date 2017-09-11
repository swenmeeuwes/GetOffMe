using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesManager {
    private static GooglePlayServicesManager _instance;

    public static GooglePlayServicesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GooglePlayServicesManager();
            return _instance;
        }
    }
    
    public void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Debugging
        PlayGamesPlatform.DebugLogEnabled = false;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public bool PromptAuthentication()
    {
        Social.localUser.Authenticate((bool success) => {
            GameManager.Instance.PlayerIsAuthenticated = success;
        });

        return GameManager.Instance.PlayerIsAuthenticated;
    }
    
    public bool ReportScoreToLeaderboard(string leaderboard, long score)
    {
        var isSuccessful = false;
        Social.ReportScore(score, leaderboard, (bool success) => {
            if (!success)
                Debug.LogWarning("Unable to report high score to Google Play Services, leaderboard: " + leaderboard);

            isSuccessful = success;
        });

        return isSuccessful;
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
    }
}

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

    public bool ServiceInitialized { get; private set; }
    public bool PlayerIsAuthenticated {
        get
        {
            return PlayGamesPlatform.Instance.IsAuthenticated();
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

        ServiceInitialized = true; // Check for fail?
    }

    public bool PromptAuthentication()
    {
        Social.localUser.Authenticate((bool success) => {
            
        });

        return PlayerIsAuthenticated;
    }
    
    public bool ReportScoreToLeaderboard(string leaderboard, long score)
    {
        if (!PlayerIsAuthenticated)
            return false;

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
        if (!PlayerIsAuthenticated)
            return;

        Social.ShowLeaderboardUI();
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        if (!PlayerIsAuthenticated)
            return;

        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
    }
}

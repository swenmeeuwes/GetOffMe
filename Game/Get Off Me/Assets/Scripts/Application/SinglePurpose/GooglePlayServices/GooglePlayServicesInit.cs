using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesInit : MonoBehaviour {
	private void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            // requests an ID token be generated.  This OAuth token can be used to
            //  identify the player to other services such as Firebase.
            .RequestIdToken()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        // Debugging
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {
            GameManager.Instance.PlayerIsAuthenticated = success;

            if (success)
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlayServiceConstants.leaderboard_score);
        });
    }
}

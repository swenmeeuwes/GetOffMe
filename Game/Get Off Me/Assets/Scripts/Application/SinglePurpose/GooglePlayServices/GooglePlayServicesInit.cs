using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesInit : MonoBehaviour {
	private void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Debugging
        PlayGamesPlatform.DebugLogEnabled = false;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {
            GameManager.Instance.PlayerIsAuthenticated = success;
        });
    }
}

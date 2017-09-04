﻿using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesScripts : MonoBehaviour {
    public void AttempAuthentication()
    {
        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if(success)
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GooglePlayServiceConstants.leaderboard_score);
        });
    }
}

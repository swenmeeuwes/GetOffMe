using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesScripts : MonoBehaviour {
    public void ShowScoreLeaderBoard()
    {
        GooglePlayServicesManager.Instance.ShowLeaderboard(GooglePlayServiceConstants.leaderboard_score);
    }
}

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayServicesInit : MonoBehaviour {
	private void Start () {
        GooglePlayServicesManager.Instance.Initialize();
        GooglePlayServicesManager.Instance.PromptAuthentication();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayLoginPromptPopup : Panel
{
    public void HandleContinueButton()
    {
        GooglePlayServicesManager.Instance.PromptAuthentication();
        Deactiveate();
    }

    public void HandleCancelButton()
    {
        Deactiveate();
    }
}

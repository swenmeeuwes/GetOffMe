using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrlScript : MonoBehaviour {
    public void OpenUrl(string url) {
        Application.OpenURL(url);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : Panel {
	void Update () {
        if (GameManager.Instance.State == GameState.PAUSE) {
            Activate();
        }
        else{
            Deactivate();
        }
	}
}

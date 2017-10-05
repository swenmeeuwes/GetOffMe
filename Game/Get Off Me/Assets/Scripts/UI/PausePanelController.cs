using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelController : MonoBehaviour {

    private GameObject panel;
    
    void Start() {
        panel = transform.Find("PausePanel").gameObject;
    }
	void Update () {
        if (GameManager.Instance.State == GameState.PAUSE)
        {
            panel.GetComponent<Panel>().Activate();
        }
        else
        {
            panel.GetComponent<Panel>().Deactivate();
        }
    }
}

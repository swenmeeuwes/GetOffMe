using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Activate() { gameObject.SetActive(true);}
    public void Deactiveate() { gameObject.SetActive(false);  }
    public void Toggle() { gameObject.SetActive(!gameObject.activeSelf); }
}

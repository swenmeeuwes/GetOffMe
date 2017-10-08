using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectDelayed : MonoBehaviour {

    // Use this for initialization
    int delay = 4;
	void Start () {
        Destroy(gameObject, delay);
    }
}

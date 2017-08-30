using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : MonoBehaviour {

    public GameObject target;
    public float speed = 5f;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
}

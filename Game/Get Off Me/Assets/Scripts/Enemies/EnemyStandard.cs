using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : AbstractEnemy {

    public Enemy enemyModel;

    // Use this for initialization
    void Start () {
        target = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float step = enemyModel.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);
        target.GetComponent<Player>().OnEnemyEnter(enemyModel.health);
        //Destroy(coll.gameObject);
    }
}

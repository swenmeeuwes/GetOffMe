using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : AbstractEnemy {
    protected override void Start () {
        base.Start();
        target = GameObject.FindWithTag("Player");
	}
	
	void Update () {

        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);
        target.GetComponent<Player>().OnEnemyEnter(model.health);
    }
}

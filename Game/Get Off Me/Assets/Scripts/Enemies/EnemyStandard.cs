using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : AbstractEnemy {
    protected override void Start () {
        base.Start();
        target = GameObject.FindWithTag("Player");
	}
	
	void Update () {
        var velocityModifier = 1;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
            velocityModifier = 10;

        Vector3 direction = (target.transform.position - transform.position).normalized * velocityModifier;
        rb.AddForce(direction);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);
        target.GetComponent<Player>().OnEnemyEnter(model.health);
    }
}

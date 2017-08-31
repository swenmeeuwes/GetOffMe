using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : AbstractEnemy {
    protected override void Start () {
        base.Start();
        target = GameObject.FindWithTag("Player");
	}
	
	void Update () {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        var outsideViewport = false;
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
            outsideViewport = true;

        var velocityModifier = 1;
        if (outsideViewport)
            velocityModifier = 10;

        Vector3 direction = (target.transform.position - transform.position).normalized * velocityModifier;
        rb.AddForce(direction);
        if (!outsideViewport && rb.velocity.magnitude < maxMagnitude)
        {
            rb.AddForce(-direction * 1000);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);

        var player = coll.gameObject.GetComponent<Player>();
        if(player)
            player.OnEnemyEnter(model.health);
    }
}

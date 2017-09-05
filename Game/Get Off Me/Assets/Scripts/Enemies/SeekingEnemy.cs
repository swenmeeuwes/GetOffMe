﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enemy which will attempt to reach the player
/// </summary>
public class SeekingEnemy : AbstractEntity {
    private GameObject target;

    protected override void Start () {
        base.Start();

        target = GameObject.FindWithTag("Player");
    }
	
	void Update () {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * model.speed);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);

        var player = coll.gameObject.GetComponent<Player>();
        if(player)
            player.OnEnemyEnter(model.health);
    }
}
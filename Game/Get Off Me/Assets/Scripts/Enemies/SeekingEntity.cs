using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enemy which will attempt to reach the player
/// </summary>
public abstract class SeekingEntity : AbstractEntity {
    protected GameObject target;

    protected override void Start () {
        base.Start();
        target = GameObject.FindWithTag("Player");
    }
	protected override void UpdateEntity () {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * model.speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity which will attempt to reach the player
/// </summary>
public abstract class SeekingEntity : AbstractEntity {
    protected GameObject target;

    protected override void Start () {
        base.Start();
        target = GameObject.FindWithTag("Player");
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void UpdateEntity () {
        Seek();
    }

    protected virtual void Seek()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * amplifiedSpeed * Time.deltaTime);
    }
}

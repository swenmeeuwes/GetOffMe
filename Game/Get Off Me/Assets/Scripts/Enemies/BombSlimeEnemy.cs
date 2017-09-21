using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSlimeEnemy : SeekingEntity {

    // Use this for initialization


    public float magnitudeForExplode = 1.0f;
    public float explodeRadius = 3.0f;

    protected override void Awake()
    {
        base.Awake();

        IgnoreTap = true;
    }

    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void UpdateEntity () {
        base.UpdateEntity();
        
	}
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        Debug.Log(coll.relativeVelocity.magnitude);
        if (coll.relativeVelocity.magnitude > magnitudeForExplode) {
            Explode();
        }
    }
    void Explode() {
        Debug.Log("Explode");
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < objects.Length; i++) {
            if (Vector2.Distance(transform.position, objects[i].transform.position) < explodeRadius) {
                StartCoroutine(objects[i].GetComponent<AbstractEntity>().Die());
            }
        }
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
    public override void Accept(IVial vial)
    {
        //vial.Apply(this);
    }
}

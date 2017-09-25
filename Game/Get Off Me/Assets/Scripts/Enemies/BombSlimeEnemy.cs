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

        
        AbstractEntity entity = coll.gameObject.GetComponent<AbstractEntity>();
        if (entity) {
            if (coll.relativeVelocity.magnitude > magnitudeForExplode)
            {
                Explode();
            }
        }
    }
    void Explode() {

        var explosionPrefab = Resources.Load<GameObject>("Enemy/Props/Explosion");
        var explosionObject = Instantiate(explosionPrefab);

        var parent = new GameObject();
        parent.AddComponent<DeleteObjectDelayed>();
        parent.transform.position = transform.position;

        explosionObject.transform.SetParent(parent.transform);
        explosionObject.transform.localPosition = new Vector3(0.0f, -0.14f, 0.0f);
        explosionObject.transform.localScale = new Vector3(explodeRadius * 2, explodeRadius * 2, explodeRadius * 2);
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
    public override void Accept(IVial vial)
    {
        vial.Apply(this);
    }
}

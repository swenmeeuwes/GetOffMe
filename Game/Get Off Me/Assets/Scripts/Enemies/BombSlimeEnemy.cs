using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSlimeEnemy : SeekingEntity {

    public float magnitudeForExplode = 1.0f;
    public float explodeRadius = 3.0f;

    private GameObject explosionObject;

    protected override void Awake()
    {
        base.Awake();
        entityType = EntityType.SLIME_BOMB;
        IgnoreTap = true;
        ComboEnabled = false;
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);

        
        AbstractEntity entity = coll.gameObject.GetComponent<AbstractEntity>();
        if (entity) {
            if (coll.relativeVelocity.magnitude > magnitudeForExplode && ScreenUtil.WorldPositionIsInView(transform.position))
            {
                Explode();
            }
        }
    }
    private void Explode() {
        var explosionPrefab = Resources.Load<GameObject>("Enemy/Props/Explosion");
        explosionObject = Instantiate(explosionPrefab);

        var parent = new GameObject();
        parent.transform.position = transform.position;
        parent.name = "Explosion container";

        explosionObject.transform.SetParent(parent.transform);
        explosionObject.transform.localPosition = new Vector3(0.0f, -0.14f, 0.0f);
        explosionObject.transform.localScale = new Vector3(explodeRadius * 2, explodeRadius * 2, explodeRadius * 2);
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
    protected override void TrackDeath()
    {
        
    }
    public override void Accept(IVial vial)
    {
        vial.Apply(this);
    }
}

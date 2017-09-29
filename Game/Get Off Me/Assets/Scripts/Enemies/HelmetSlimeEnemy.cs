using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSlimeEnemy : SeekingEntity {

    bool hasHelmet;

    public int neededTapsForHelmet;

    public int pointsForHelmetTap;

    protected override void Awake()
    {
        base.Awake();
        entityType = EntityType.SLIME_HELMET;

        if (pointsForHelmetTap <= 0)
            pointsForHelmetTap = 1;

        neededTapsForHelmet = 1;
        pointsForHelmetTap = 1;

        ShowParticles = false;
        hasHelmet = true;
        Draggable = false;
    }

    protected override void Start() {
        base.Start();
    }
    protected override void OnTap() {
        if (hasHelmet)
        {
			neededTapsForHelmet--;
            if (neededTapsForHelmet <= 0)
            {
                hasHelmet = false;
                IgnoreTap = true;

                // Create flipped particle
                var helmetPrefab = Resources.Load<GameObject>("Enemy/Props/Helmet");
                var helmetObject = Instantiate(helmetPrefab);

                var parent = new GameObject();
                parent.AddComponent<DeleteObjectDelayed>();
                parent.transform.position = transform.position;

                helmetObject.transform.position = Vector3.zero;
                helmetObject.transform.SetParent(parent.transform);

                animator.SetTrigger("loseHelmet");
                Draggable = true;
                ShowParticles = true;
            }     

            int addedScore = comboSystem.AwardPoints(pointsForHelmetTap);
            HandleScore(addedScore);

            //HandleCombo(); // Don't count HELMET taps towards combo count
        }
        base.OnTap();
    }

    protected override void OnSwipe(Vector3 swipeVector) {
        if (hasHelmet)
            return;
        base.OnSwipe(swipeVector);
    }

    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health + (hasHelmet ? 1: 0)); // TODO Temporary extra health for helmet
        base.OnPlayerHit(player);
    }
	public override void Accept (IVial vial)
	{
		vial.Apply (this);
	}
}

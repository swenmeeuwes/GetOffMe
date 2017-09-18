using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSlimeEnemy : SeekingEntity {

    bool hasHelmet;

    public int NeededTapsForHelmet { get; set; }

    public int pointsForHelmetTap;

    private void Awake()
    {
        base.Awake();

        if (pointsForHelmetTap <= 0)
            pointsForHelmetTap = 1;

        NeededTapsForHelmet = 1;

        ShowParticles = false;
        hasHelmet = true;
        Draggable = false;
    }

    protected override void Start() {
        base.Start();
		

        Debug.Log("Just SPawned helmet enemy with: "+NeededTapsForHelmet);
    }
    public override void OnTap() {
        if (hasHelmet)
        {
			NeededTapsForHelmet--;
            Debug.Log("Tapped -> " + NeededTapsForHelmet);
			if (NeededTapsForHelmet <= 0) {
				hasHelmet = false;
			}
			actionRewardsCombo = true;
            ShowParticles = true;
            Draggable = true;
            animator.SetTrigger("loseHelmet");

            // Create flipped particle
            var helmetPrefab = Resources.Load<GameObject>("Enemy/Props/Helmet");
            var helmetObject = Instantiate(helmetPrefab);

            var parent = new GameObject();
            parent.AddComponent<DeleteObjectDelayed>();
            parent.transform.position = transform.position;

            helmetObject.transform.position = Vector3.zero;
            helmetObject.transform.SetParent(parent.transform);

            if (GameManager.Instance.State == GameState.PLAY)
            {
				int addedScore = comboSystem.AwardPoints(pointsForHelmetTap);
				FindObjectOfType<ScoreParticleManager>().ShowRewardIndicatorAt(addedScore, transform.position, true);
            }
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
        player.AbsorbEnemy(model.health + (hasHelmet?1: 0)); // TODO Temporary extra health for helmet
        base.OnPlayerHit(player);
    }
	public override void Accept (IVial vial)
	{
		vial.Apply (this);
	}
}

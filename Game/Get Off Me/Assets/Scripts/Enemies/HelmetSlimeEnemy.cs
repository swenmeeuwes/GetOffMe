using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSlimeEnemy : SeekingEntity {

    bool hasHelmet;

    protected override void Start() {
        base.Start();
        ShowParticles = false;
        hasHelmet = true;
        Draggable = false;
    }
    public override void OnTap() {
        if (hasHelmet)
        {
			actionRewardsCombo = true;
            hasHelmet = false;
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
				int addedScore = comboSystem.AwardPoints(1);
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
}

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
            hasHelmet = false;
            ShowParticles = true;
            Draggable = true;
            animator.SetTrigger("loseHelmet");

            // Create flipped particle
            var helmetPrefab = Resources.Load<GameObject>("Enemy/Props/Helmet");
            var helmetObject = Instantiate(helmetPrefab);

            helmetObject.transform.position = transform.position;
            helmetObject.transform.SetParent(transform);
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
        Dispatch("dying", this);
        OnEntityDestroy();
    }
}

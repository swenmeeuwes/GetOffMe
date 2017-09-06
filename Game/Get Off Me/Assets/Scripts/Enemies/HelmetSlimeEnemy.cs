using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSlimeEnemy : SeekingEntity {

    bool hasHelmet;

    protected override void Start() {
        base.Start();
        hasHelmet = true;
    }
    public override void OnTap() {
        if (hasHelmet)
        {
            hasHelmet = false;
            //TODO flipoff animation
        }
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        Dispatch("dying", this);
        OnDestroy();
    }
}

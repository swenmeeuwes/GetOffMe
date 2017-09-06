using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlimeAlly : SeekingEntity
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPlayerHit(Player player)
    {
        //player.AbsorbEnemy(model.health);
        Destroy(gameObject);
    }
}



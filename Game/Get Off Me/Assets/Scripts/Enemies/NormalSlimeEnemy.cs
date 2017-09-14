using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlimeEnemy : SeekingEntity
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
	public override void Accept (IVial vial)
	{
		vial.Apply (this);
	}
}


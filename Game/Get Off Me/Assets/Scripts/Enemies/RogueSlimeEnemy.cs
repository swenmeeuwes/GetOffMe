using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSlimeEnemy : SeekingEntity {

	protected override void Start()
	{
		base.Start();
	}

    protected override void Awake()
    {
        base.Awake();
        entityType = EntityType.SLIME_ROGUE;
        IgnoreTap = true;
    }

    public override void OnPlayerHit(Player player)
	{
		player.AbsorbEnemy(Model.health);
		base.OnPlayerHit(player);
	}

	public override void Accept (IVial vial)
	{
		vial.Apply (this);
	}
}

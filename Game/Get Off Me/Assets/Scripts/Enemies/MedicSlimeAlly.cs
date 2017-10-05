using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MedicSlimeAlly : SeekingEntity {
	private int healAmount;
    protected override void Awake()
    {
        base.Awake();
        healAmount = 1;
        entityType = EntityType.SLIME_MEDIC;
        IgnoreTap = true;
    }
    protected override void Start(){
		base.Start ();
	}
    public override void OnPlayerHit(Player player)
    {
		player.Heal(healAmount);
        base.OnPlayerHit(player);
    }
	public override void Accept (IVial vial)
	{
		vial.Apply (this);
	}

    protected override void HandleScore()
    {
        // NO SCORE
    }

    protected override void HandleCombo()
    {
        // NO COMBO
    }

    protected override void HandleCloseCallText()
    {
        // NO CLOSE CALL TEXT
    }
}

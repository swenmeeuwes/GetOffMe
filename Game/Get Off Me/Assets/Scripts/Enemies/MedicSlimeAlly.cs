using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MedicSlimeAlly : SeekingEntity {
	private int healAmount;

	protected override void Start(){
		base.Start ();
		healAmount = 1;
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
}

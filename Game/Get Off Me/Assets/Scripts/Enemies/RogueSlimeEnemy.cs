using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSlimeEnemy : SeekingEntity {

	protected override void Start()
	{
		base.Start();
	}
	public override void OnPlayerHit(Player player)
	{
		player.AbsorbEnemy(model.health);
		base.OnPlayerHit(player);
	}
	public void Configure(int pointModifier, int speedModifier){
		base.Configure(pointModifier);
		model.speed += speedModifier;
	}
}

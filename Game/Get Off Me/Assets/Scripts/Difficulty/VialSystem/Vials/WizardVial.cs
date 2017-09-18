﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardVial : IVial {
	private const int pointModifier = 1;
	private const int healthModifier = 2;
	private const float channelTimeModifier = -0.5f;
	public void Apply (HelmetSlimeEnemy entity){}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){
		entity.model.awardPoints += pointModifier;
		entity.model.health += healthModifier;
		entity.channelTime += channelTimeModifier;
	}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){}
	public void Apply (ComboSystem comboSystem){}
	public void Apply (OffScreenSpawner spawner){}
}
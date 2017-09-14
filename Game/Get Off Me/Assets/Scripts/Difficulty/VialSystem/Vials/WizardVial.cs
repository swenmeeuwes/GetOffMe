using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardVial : IVial {
	private const int pointModifier = 1;
	private const int healthModifier = 2;
	private const float channelTimeModifier = -0.5f;
	void Apply (HelmetSlimeEnemy entity){}
	void Apply (NormalSlimeEnemy entity){}
	void Apply (RogueSlimeEnemy entity){}
	void Apply (WizardSlimeEnemy entity){
		entity.model.awardPoints += pointModifier;
		entity.model.health += healthModifier;
		entity.channelTime += channelTimeModifier;
	}
	void Apply (MedicSlimeAlly entity){}
	void Apply (Player player){}
	void Apply (ComboSystem comboSystem){}
	void Apply (OffScreenSpawner spawner){}
}

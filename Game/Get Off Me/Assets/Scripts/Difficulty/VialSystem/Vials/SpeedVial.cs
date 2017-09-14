using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedVial : IVial {
	private const float speedModifier = 0.5f;
	private const float doubleComboChanceModifier = 20;

	void Apply (HelmetSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	void Apply (NormalSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	void Apply (RogueSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	void Apply (WizardSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	void Apply (MedicSlimeAlly entity){
		entity.model.speed += speedModifier;
	}
	void Apply (Player player){}
	void Apply (ComboSystem comboSystem){
		comboSystem.chanceAtDoubleCombo += doubleComboChanceModifier;
	}
	void Apply (OffScreenSpawner spawner){}
}
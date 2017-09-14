using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedVial : IVial {
	private const float speedModifier = 0.5f;
	private const float doubleComboChanceModifier = 20;

	public void Apply (HelmetSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	public void Apply (NormalSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	public void Apply (RogueSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	public void Apply (WizardSlimeEnemy entity){
		entity.model.speed += speedModifier;
	}
	public void Apply (MedicSlimeAlly entity){
		entity.model.speed += speedModifier;
	}
	public void Apply (Player player){}
	public void Apply (ComboSystem comboSystem){
		comboSystem.chanceAtDoubleCombo += doubleComboChanceModifier;
	}
	public void Apply (OffScreenSpawner spawner){}
}
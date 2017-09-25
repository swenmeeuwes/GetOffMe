using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedVial : IVial {
	private const float speedModifier = 1.0f * 60;
	private const float doubleComboChanceModifier = 30;

	public void Apply(HelmetSlimeEnemy entity){
		entity.amplifiedSpeed += speedModifier;
	}
	public void Apply(NormalSlimeEnemy entity){
		entity.amplifiedSpeed += speedModifier;
	}
	public void Apply(RogueSlimeEnemy entity){
		entity.amplifiedSpeed += speedModifier;
	}
	public void Apply(WizardSlimeEnemy entity){
		entity.amplifiedSpeed += speedModifier;
	}
	public void Apply(MedicSlimeAlly entity){
		entity.amplifiedSpeed += speedModifier;
	}
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player){}
	public void Apply(ComboSystem comboSystem){
		comboSystem.chanceAtDoubleCombo += doubleComboChanceModifier;
	}
	public void Apply(OffScreenSpawner spawner) { }
    public void Apply(List<GamePhase> phases) { }
}
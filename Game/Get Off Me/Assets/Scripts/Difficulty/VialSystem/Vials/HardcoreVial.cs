using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcoreVial : IVial {
	private int playerHealthModifier = -5;
	private int absorbPercentageModifier = 30;

	void Apply (HelmetSlimeEnemy entity){}
	void Apply (NormalSlimeEnemy entity){}
	void Apply (RogueSlimeEnemy entity){}
	void Apply (WizardSlimeEnemy entity){}
	void Apply (MedicSlimeAlly entity){}
	void Apply (Player player){
		player.health += playerHealthModifier;
		player.absorbPercentage += absorbPercentageModifier;
	}
	void Apply (ComboSystem comboSystem){}
	void Apply (OffScreenSpawner spawner){}
}

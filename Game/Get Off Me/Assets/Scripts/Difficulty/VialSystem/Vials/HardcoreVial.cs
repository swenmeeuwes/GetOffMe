using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcoreVial : IVial {
	private int playerHealthModifier = -5;
	private int absorbPercentageModifier = 30;

	public void Apply (HelmetSlimeEnemy entity){}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){
		player.health += playerHealthModifier;
		player.absorbPercentage += absorbPercentageModifier;
	}
	public void Apply (ComboSystem comboSystem){}
	public void Apply (OffScreenSpawner spawner){}
}

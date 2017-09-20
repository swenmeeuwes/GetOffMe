using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcoreVial : IVial {
	private int playerHealthModifier = -5;
    private float comboSystemRadiusMultiplier = 1.2f;

	public void Apply (HelmetSlimeEnemy entity){}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){
		player.health += playerHealthModifier;
	}
	public void Apply (ComboSystem comboSystem){
        comboSystem.originalRadius *= comboSystemRadiusMultiplier;
    }
	public void Apply (OffScreenSpawner spawner){}
}

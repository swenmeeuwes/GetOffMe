using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetVial : IVial {
	private const int tapsModifier = 1;
	private const int pointForHelmetTapModifier = 1;

	void Apply (HelmetSlimeEnemy entity){
		entity.neededTapsForHelmet += tapsModifier;
		entity.pointsForHelmetTap += pointForHelmetTapModifier;
	}
	void Apply (NormalSlimeEnemy entity){}
	void Apply (RogueSlimeEnemy entity){}
	void Apply (WizardSlimeEnemy entity){}
	void Apply (MedicSlimeAlly entity){}
	void Apply (Player player){}
	void Apply (ComboSystem comboSystem){}
	void Apply (OffScreenSpawner spawner){}
}
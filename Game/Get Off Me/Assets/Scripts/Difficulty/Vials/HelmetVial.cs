using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetVial : IVial {
	private const int tapsIncrease = 1;
	void Apply (HelmetSlimeEnemy entity){
		entity.neededTapsForHelmet += tapsIncrease;
	}
	void Apply (NormalSlimeEnemy entity){}
	void Apply (RogueSlimeEnemy entity){}
	void Apply (WizardSlimeEnemy entity){}
	void Apply (MedicSlimeAlly entity){}
}
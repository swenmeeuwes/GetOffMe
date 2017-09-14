using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueVial : IVial {
	private const float speedIncrease = 1.0f;
	void Apply (HelmetSlimeEnemy entity){}
	void Apply (NormalSlimeEnemy entity){}
	void Apply (RogueSlimeEnemy entity){
		entity.model.speed += speedIncrease;
	}
	void Apply (WizardSlimeEnemy entity){}
	void Apply (MedicSlimeAlly entity){}
}

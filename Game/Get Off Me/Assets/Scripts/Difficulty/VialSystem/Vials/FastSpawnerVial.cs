using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSpawnerVial : IVial {
	private const float spawnerCurveModifier = 0.2f;
	public void Apply (HelmetSlimeEnemy entity){}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){}
	public void Apply (ComboSystem comboSystem){}
	public void Apply (OffScreenSpawner spawner){
		for(int i = 0; i < spawner.spawnRateCurve.keys.Length; i ++){
			spawner.spawnRateCurve.keys[i].value += spawnerCurveModifier;
		}
	}
}

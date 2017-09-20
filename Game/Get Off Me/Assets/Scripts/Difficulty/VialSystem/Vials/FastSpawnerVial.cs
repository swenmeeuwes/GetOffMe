using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSpawnerVial : IVial {
	private const float spawnerCurveModifier = -0.3f;
	public void Apply (HelmetSlimeEnemy entity){}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){}
	public void Apply (ComboSystem comboSystem){}
	public void Apply (OffScreenSpawner spawner){
        Debug.Log("spawner");

        AnimationCurve newCurve;
        Keyframe[] frames = new Keyframe[spawner.spawnRateCurve.keys.Length];

		for(int i = 0; i < spawner.spawnRateCurve.keys.Length; i ++){
            frames[i] = new Keyframe(spawner.spawnRateCurve.keys[i].time, spawner.spawnRateCurve.keys[i].value += spawnerCurveModifier);
        }
        newCurve = new AnimationCurve(frames);

        spawner.spawnRateCurve = newCurve;
	}
}

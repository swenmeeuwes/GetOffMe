using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetVial : IVial {
	private const int tapsModifier = 4;
	private const int pointForHelmetTapModifier = 1;

	public void Apply (HelmetSlimeEnemy entity){
        entity = (HelmetSlimeEnemy)entity;
        Debug.Log("Initial Taps Needed according to Vial: " + entity.NeededTapsForHelmet);
        Debug.Log("Adding in vial: " + tapsModifier);
		entity.NeededTapsForHelmet += tapsModifier;
        Debug.Log("New Taps Needed according to Vial: " + entity.NeededTapsForHelmet);

		entity.pointsForHelmetTap += pointForHelmetTapModifier;
	}
	public void Apply (NormalSlimeEnemy entity){}
	public void Apply (RogueSlimeEnemy entity){}
	public void Apply (WizardSlimeEnemy entity){}
	public void Apply (MedicSlimeAlly entity){}
	public void Apply (Player player){}
	public void Apply (ComboSystem comboSystem){}
	public void Apply (OffScreenSpawner spawner){}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcoreVial : IVial {
	private int playerHealthModifier = -5;
    private float comboSystemRadiusMultiplier = 1.2f;
    private float medicWeightModifier = 5;

	public void Apply(HelmetSlimeEnemy entity) { }
	public void Apply(NormalSlimeEnemy entity) { }
	public void Apply(RogueSlimeEnemy entity) { }
	public void Apply(WizardSlimeEnemy entity) { }
	public void Apply(MedicSlimeAlly entity) { }
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player){
		player.health += playerHealthModifier;
        player.maxHealth = player.health;
	}
	public void Apply(ComboSystem comboSystem){
        //comboSystem.originalRadius *= comboSystemRadiusMultiplier; // HAS TO BE REIMPLEMENTED
    }
	public void Apply(OffScreenSpawner spawner) { }
    public void Apply(List<GamePhase> phases) {
        for (int i = 0; i < phases.Count; i++) {
            for (int j = 0; j < phases[i].objectKeys.Count; j++) {
                if (phases[i].objectKeys[j].GetComponent<MedicSlimeAlly>() != null) {
                    phases[i].weights[j] += medicWeightModifier;
                    break;
                }
            }
        }
    }
}

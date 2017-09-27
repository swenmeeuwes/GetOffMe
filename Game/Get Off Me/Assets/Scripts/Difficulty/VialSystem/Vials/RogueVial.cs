using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueVial : IVial {
	private const float speedIncrease = 1.0f;
    private const int pointModifier = 1;

	public void Apply(HelmetSlimeEnemy entity) { }
	public void Apply(NormalSlimeEnemy entity) { }
	public void Apply(RogueSlimeEnemy entity){
		entity.model.speed += speedIncrease;
        entity.model.awardPoints += pointModifier;
	}
	public void Apply(WizardSlimeEnemy entity) { }
	public void Apply(MedicSlimeAlly entity) { }
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player){}
	public void Apply(ComboSystem comboSystem){}
	public void Apply(OffScreenSpawner spawner){}
    public void Apply(List<GamePhase> phases) { }
}

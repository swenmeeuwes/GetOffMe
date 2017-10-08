using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueVial : IVial {
	private const float speedIncrease = 1.5f;
    private const int pointModifier = 2;

	public void Apply(HelmetSlimeEnemy entity) { }
	public void Apply(NormalSlimeEnemy entity) { }
	public void Apply(RogueSlimeEnemy entity){
		entity.Model.speed += speedIncrease;
        entity.Model.awardPoints += pointModifier;
	}
	public void Apply(WizardSlimeEnemy entity) { }
	public void Apply(MedicSlimeAlly entity) { }
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player){}
	public void Apply(ComboSystem comboSystem){}
	public void Apply(OffScreenSpawner spawner){}
    public void Apply(List<GamePhase> phases) { }
}

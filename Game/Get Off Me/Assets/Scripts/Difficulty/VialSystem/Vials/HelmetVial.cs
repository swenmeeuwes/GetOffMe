using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetVial : IVial {
	private const int tapsModifier = 1;
	private const int pointForHelmetTapModifier = 1;

	public void Apply (HelmetSlimeEnemy entity){
		entity.neededTapsForHelmet += tapsModifier;
		entity.pointsForHelmetTap += pointForHelmetTapModifier;
	}
	public void Apply(NormalSlimeEnemy entity) { }
	public void Apply(RogueSlimeEnemy entity) { }
	public void Apply(WizardSlimeEnemy entity) { }
	public void Apply(MedicSlimeAlly entity) { }
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player) { }
	public void Apply(ComboSystem comboSystem) { }
	public void Apply(OffScreenSpawner spawner) { }
    public void Apply(List<GamePhase> phases) { }
}
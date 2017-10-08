using System.Collections.Generic;

public class SpeedVial : IVial {
	private const float speedModifier = 0.7f;
	private const float doubleComboChanceModifier = 30;

	public void Apply(HelmetSlimeEnemy entity){
		entity.Model.speed += speedModifier;
	}
	public void Apply(NormalSlimeEnemy entity){
		entity.Model.speed += speedModifier;
	}
	public void Apply(RogueSlimeEnemy entity){
		entity.Model.speed += speedModifier;
	}
	public void Apply(WizardSlimeEnemy entity){
		entity.Model.speed += speedModifier;
	}
	public void Apply(MedicSlimeAlly entity){
		entity.Model.speed += speedModifier;
	}
    public void Apply(BombSlimeEnemy entity) { }

	public void Apply(Player player){}
	public void Apply(ComboSystem comboSystem){
		comboSystem.chanceAtDoubleCombo += doubleComboChanceModifier;
	}
	public void Apply(OffScreenSpawner spawner) { }
    public void Apply(List<GamePhase> phases) { }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVial {
	void Apply (HelmetSlimeEnemy entity);
	void Apply (NormalSlimeEnemy entity);
	void Apply (RogueSlimeEnemy entity);
	void Apply (WizardSlimeEnemy entity);
	void Apply (MedicSlimeAlly entity);
	void Apply (Player player);
	void Apply (ComboSystem comboSystem);
	void Apply (OffScreenSpawner spawner);
}

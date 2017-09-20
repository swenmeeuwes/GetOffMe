using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VialSystem : MonoBehaviour {

	private Player player;
	private ComboSystem comboSystem;
	private OffScreenSpawner spawner;

	void Start () {
		player = GameObject.Find ("Player").GetComponent<Player> ();
		comboSystem = GameObject.Find ("ComboSystem").GetComponent<ComboSystem> ();
		spawner = GameObject.Find ("Spawner").GetComponent<OffScreenSpawner> ();

		var activeVials = VialManager.Instance.GetActiveVials ();
		for(int i = 0; i < activeVials.Count; i ++){
			activeVials [i].Apply (player);
			activeVials [i].Apply (comboSystem);
			activeVials [i].Apply (spawner);
		}
	}
}

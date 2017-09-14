using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VialManager {

	private static VialManager _instance;
	public static VialManager Instance {
		get {
			if (_instance == null)
				_instance = new VialManager();
			return _instance;
		}
	}
	public Dictionary<string, IVial> vials;
	private VialManager(){
		var difficultyModifierNames = GameManager.Instance.SaveGame.DifficultyModifiers.Select ((modifier) => modifier.Name).ToArray();

		vials = new Dictionary<string, IVial> ();
		vials.Add("")
	}
}

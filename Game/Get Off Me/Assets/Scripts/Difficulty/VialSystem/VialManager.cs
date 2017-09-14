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
		vials = new Dictionary<VialType, IVial> ();
		vials.Add (VialType.SPEED_VIAL, new SpeedVial ());
		vials.Add (VialType.SPAWN_VIAL, new FastSpawnerVial ());
		vials.Add (VialType.HELMET_VIAL, new HelmetVial ());
		vials.Add (VialType.ROGUE_VIAL, new RogueVial ());
		vials.Add (VialType.WIZARD_VIAL, new WizardVial ());
		vials.Add (VialType.HARDCORE_VIAL, new HardcoreVial ());
	}
}

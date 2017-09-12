using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StopParticleSystem : MonoBehaviour {
	void Start () {
        gameObject.GetComponent<ParticleSystem>().Stop();	
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour {


	[SerializeField]
	private Camera orthographicCamera;

	public float radius;
	public int Combo { get; set; }

	// Use this for initialization
	void Start () {
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

		if (radius == 0) {
			radius = 3.0f;
		}
	}
	public void Increase(int addValue){
		Combo += addValue;
		Debug.Log (Combo);
	}
	public void Reset(){
		Combo = 0;
	}

	public int AwardPoints(int score){
		int addScore = (score + Combo);
		ScoreManager.Instance.Score += addScore; 
		return addScore;
	}
	public bool CheckIfCombo(Vector3 enemyPosition){
		return (Vector3.Distance (transform.position, enemyPosition) < radius);
	}
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{
		if (orthographicCamera != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}
}

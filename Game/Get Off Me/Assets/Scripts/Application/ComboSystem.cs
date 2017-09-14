using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour {
    [SerializeField]
    private Image comboRadiusIndicator;
	[SerializeField]
	private Camera orthographicCamera;

	private float radius;
	public float originalRadius;
	public int Combo { get; set; }

	// Use this for initialization
	void Start () {
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

		if (originalRadius == 0) {
			originalRadius = 3.0f;
		}
        SetScale(1);
    }
	public void Increase(int addValue){
		Combo += addValue;
	}
	public void SetScale(float size){
		radius = originalRadius * size;

        if (comboRadiusIndicator) {
            var diameter = radius * 2;
            comboRadiusIndicator.rectTransform.sizeDelta = Vector2.one * diameter;
        }
	}
	public void Reset(){
		Combo = 0;
	}

	public int AwardPoints(int score){
		int addScore = (score + Combo);
		ScoreManager.Instance.Score += addScore; 
		return addScore;
	}
	public bool CheckIfCombo(Vector2 enemyPosition){
		return (Vector2.Distance (transform.position, enemyPosition) < radius);
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

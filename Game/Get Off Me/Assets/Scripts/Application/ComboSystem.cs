using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    [SerializeField]
    private Image comboRadiusIndicator;
    [SerializeField]
    private Camera orthographicCamera;
    [SerializeField]
    private Text encouragementTextField;

    private float radius;
    public float originalRadius;
    public int Combo { get; set; }

	[HideInInspector]
	public float chanceAtDoubleCombo;

    // Use this for initialization
    void Awake() {
        if (originalRadius == 0)
        {
            originalRadius = 3.0f;
        }
        chanceAtDoubleCombo = 0;
    }
	void Start () {
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

        encouragementTextField.gameObject.SetActive(false);

        SetScale(1);
    }
	public void Increase(int addValue){
		Combo += addValue;
		if (Random.Range (1.0f, 100.0f) < chanceAtDoubleCombo)
			Combo += addValue;

		comboRadiusIndicator.color = ComboColorResolver.Resolve(Combo, 0.1f);
	}
	public void SetScale(float size){
		radius = originalRadius * size;

        if (comboRadiusIndicator)
        {
            var diameter = radius * 2;
            comboRadiusIndicator.rectTransform.sizeDelta = Vector2.one * diameter;
        }
    }

    public void ShowEncouragement(string text)
    {
        encouragementTextField.text = text;
        encouragementTextField.gameObject.SetActive(true);
    }

    public void HideEncouragement()
    {
        encouragementTextField.gameObject.SetActive(false);
    }

    public void Reset()
    {
        Combo = 0;
        comboRadiusIndicator.color = new Color(17f / 255f, 17f / 255f, 17f / 255f, 1);
    }

    public int AwardPoints(int score)
    {
        int addScore = (score + Combo);
        ScoreManager.Instance.Score += addScore;
        return addScore;
    }

    public bool CheckIfCombo(Vector2 enemyPosition)
    {
        return (Vector2.Distance(transform.position, enemyPosition) < radius);
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

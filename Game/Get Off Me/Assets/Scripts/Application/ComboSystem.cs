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
    [SerializeField]
    private ComboCircle comboCircle;
    [SerializeField]
    private int ComboNeededForNextTier = 5;

    private float radius;

    private int m_Combo;
    public int Combo {
        get
        {
            return m_Combo;
        }
        private set
        {
            m_Combo = value;
            HandleComboCountChanged();
        }
    }

	[HideInInspector]
	public float chanceAtDoubleCombo;
    //public float originalRadius;

    // Use this for initialization
    void Awake() {
        //if (originalRadius == 0)
        //{
        //    originalRadius = 3.0f;
        //}
        chanceAtDoubleCombo = 0;
    }

	void Start () {
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

        encouragementTextField.gameObject.SetActive(false);

        //SetScale(1);
    }

	public void Increase(int addValue){
		Combo += addValue;
		if (Random.Range (1.0f, 100.0f) < chanceAtDoubleCombo)
			Combo += addValue;

        //comboRadiusIndicator.color = ComboColorResolver.Resolve(Combo, 0.1f);
    }

	//public void SetScale(float size){
		//radius = originalRadius * size;
 //       //comboCircle.Radius = radius;


 //       //if (comboRadiusIndicator)
 //       //{
 //       //    var diameter = radius * 2;
 //       //    comboRadiusIndicator.rectTransform.sizeDelta = Vector2.one * diameter;
 //       //}
 //   }

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
        comboCircle.Color = new Color(17f / 255f, 17f / 255f, 17f / 255f, 1);
        Camera.main.backgroundColor = Color.black;

        //comboRadiusIndicator.color = new Color(17f / 255f, 17f / 255f, 17f / 255f, 1);
    }

    public int AwardPoints(int score)
    {
        int addScore = (score + Combo);
        ScoreManager.Instance.Score += addScore;
        return addScore;
    }

    public bool IntersectsComboCircle(Vector2 position)
    {
        return comboCircle.Intersects(position);
    }

    private void HandleComboCountChanged()
    {
        comboCircle.Color = ComboColorResolver.Resolve(Combo, 0.7f);        
        Camera.main.backgroundColor = ComboColorResolver.Resolve(Combo, 0.085f);

        var residu = Combo % ComboNeededForNextTier;
        comboCircle.DistortingScale = 1 / (ComboNeededForNextTier / (float)residu);

        if (residu == 0)
            comboCircle.Keyframe = Combo;
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

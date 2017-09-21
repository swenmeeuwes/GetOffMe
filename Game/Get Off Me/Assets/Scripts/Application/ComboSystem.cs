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
    [SerializeField]
    private string[] encouragementTexts;

    private float radius;
    public float comboSizeCurveModifier = 1;
    private ParticleSystem particles;

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

    void Awake() {
        chanceAtDoubleCombo = 0;
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
    }

	void Start () {
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

        encouragementTextField.gameObject.SetActive(false);
    }

	public void Increase(int addValue){
		Combo += addValue;
		if (Random.Range (1.0f, 100.0f) < chanceAtDoubleCombo)
			Combo += addValue;
    }

    public void ShowEncouragement(string text, bool randomizeColor = true)
    {
        if(randomizeColor)
            encouragementTextField.color = new Color(Random.value, Random.value, Random.value);
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

        if (Combo > 0 && residu == 0)
        {
            ShowEncouragement(encouragementTexts[Mathf.FloorToInt(Random.value * encouragementTexts.Length)] + "!");
            CancelInvoke("HideEncouragement");
            Invoke("HideEncouragement", 2f);

            //if (Combo != 0) {
            //    ParticleSystem.ShapeModule shapeModule = particles.shape;
            //    shapeModule.radius = comboCircle.Radius;

            //    particles.Emit(60);
            //}
            

            comboCircle.Keyframe = (Combo * comboSizeCurveModifier);
        }
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

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
    private Text comboStreakTextField;
    [SerializeField]
    private Text encouragementTextField;
    [SerializeField]
    private ComboCircle comboCircle;
    [SerializeField]
    private int ComboNeededForNextTier = 5;
    [SerializeField]
    private string[] encouragementTexts;
    [SerializeField]
    private AnimationCurve comboScoreRatio;

    private int currentComboTier = 0;

    private float radius;
    private ParticleSystem particles;

    [HideInInspector]
    public float comboSizeCurveModifier = 1;
    [Tooltip("The amount of combo points the player loses on hit")]
    public int comboLosePoints = 10;

    private SoundManager soundManager;

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
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		if (orthographicCamera == null)
			orthographicCamera = Camera.main;

        encouragementTextField.gameObject.SetActive(false);
        comboStreakTextField.gameObject.SetActive(false);
    }

	public void Increase(int addValue){
        if (Mathf.FloorToInt((Combo + addValue) / ComboNeededForNextTier) != currentComboTier) {
            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.radius = comboCircle.Radius;

            particles.Emit(60);
        }

		Combo += addValue;
		if (Random.Range (1.0f, 100.0f) < chanceAtDoubleCombo)
			Combo += addValue;

    }

    public void Decrease()
    {
        if (Mathf.FloorToInt(Mathf.Max(0, Combo - comboLosePoints) / ComboNeededForNextTier) != currentComboTier)
        {
            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.radius = comboCircle.Radius;

            particles.Emit(60);
        }
        Combo = Mathf.Max(0, Combo - comboLosePoints);
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

    public void HideEncouragement(float delay)
    {
        CancelInvoke("HideEncouragement");
        Invoke("HideEncouragement", delay);
    }

    public void Reset()
    {
        Combo = 0;
        comboCircle.Color = new Color(17f / 255f, 17f / 255f, 17f / 255f, 1);
        Camera.main.backgroundColor = Color.black;
    }
    public int AwardPoints(int score)
    {
        var comboAddition = Mathf.FloorToInt(Combo * comboScoreRatio.Evaluate(Combo));
        int addScore = (score + comboAddition);
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
            HideEncouragement(2f);

            comboCircle.Keyframe = (Combo * comboSizeCurveModifier);
        }
        comboCircle.Keyframe = (Combo - residu) * comboSizeCurveModifier;


        if (Combo > 0)
            ShowComboStreak(Combo);
        else
            HideComboStreak();

        currentComboTier = Mathf.FloorToInt(Combo / ComboNeededForNextTier);
        soundManager.HandleComboTier(currentComboTier);
    }

    private void ShowComboStreak(int amount)
    {
        var randomX = Random.value * 50 - 25;
        var randomY = Random.value * 80 - 40;
        var randomZ = Random.value * 70 - 35;
        comboStreakTextField.rectTransform.localRotation = Quaternion.Euler(randomX, randomY, randomZ);
        comboStreakTextField.text = amount + "x";
        comboStreakTextField.color = ComboColorResolver.Resolve(Combo);

        comboStreakTextField.gameObject.SetActive(true);
    }

    private void HideComboStreak()
    {
        comboStreakTextField.gameObject.SetActive(false);
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

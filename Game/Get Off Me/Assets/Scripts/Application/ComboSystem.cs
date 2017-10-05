using UnityEngine;
using UnityEngine.UI;

// Move to 'Combo' folder
public class ComboSystem : EventDispatcher
{
    public const string COMBO_CHANGED = "ComboSystem.COMBO_CHANGED";

    public static ComboSystem Instance;

    [SerializeField]
    private Image comboRadiusIndicator;
    [SerializeField]
    private Camera orthographicCamera;
    [SerializeField]
    private Text comboHelpText;
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

    private ParticleSystem particles;

    [HideInInspector]
    public float comboSizeCurveModifier = 1;
    [Tooltip("The amount of combo points the player loses on hit")]
    public int comboLosePoints = 10;

    private Player player;
    public int OnFireMinimumTier = 7;

    public int MinimumComboForVial;

    [HideInInspector]
    public float startTimeUnlockVial;
    [HideInInspector]
    public bool completingVialRequirement = false;

    private int highestCombo = 0;
    private int m_Combo;
    public int Combo {
        get
        {
            return m_Combo;
        }
        private set
        {
            Dispatch(COMBO_CHANGED, new ComboChangedEvent()
            {
                OldCombo = Combo,
                NewCombo = value
            });

            m_Combo = value;
            HandleComboCountChanged();

            highestCombo = Mathf.Max(value, highestCombo);
        }
    }

    private bool _enabled;
    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            comboCircle.gameObject.SetActive(value);
            comboHelpText.gameObject.SetActive(value);
            _enabled = value;
        }
    }

	[HideInInspector]
	public float chanceAtDoubleCombo;

    protected override void Awake() {
        base.Awake();

        if (Instance != null)
            Debug.LogWarning("Another ComboSystem was already instantiated!");

        Instance = this;

        Enabled = true;

        startTimeUnlockVial = Time.time;
        chanceAtDoubleCombo = 0;
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

	void Start () {
        if (orthographicCamera == null)
			orthographicCamera = Camera.main;

        encouragementTextField.gameObject.SetActive(false);
        comboStreakTextField.gameObject.SetActive(false);
    }

	public void Increase(int addValue){
        if (!Enabled)
            return;

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
        if (!Enabled)
            return;

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
        if (!Enabled)
            return 0;

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

        comboHelpText.gameObject.SetActive(Combo == 0);

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

        player.Lit = (Combo > OnFireMinimumTier * ComboNeededForNextTier);
        
        currentComboTier = Mathf.FloorToInt(Combo / ComboNeededForNextTier);
        BackgroundMusicManager.Instance.HandleComboTier(currentComboTier);
        if (Combo >= MinimumComboForVial)
        {
            if (!completingVialRequirement) {
                startTimeUnlockVial = Time.time;
            }
            completingVialRequirement = true;
        }
        else if (highestCombo >= MinimumComboForVial)
        {
            completingVialRequirement = false;
            GameManager.Instance.HandleHighestTimeAboveHighCombo(Time.time - startTimeUnlockVial);
        }
    }
    private void ShowComboStreak(int amount)
    {
        if (!Enabled)
            return;

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEntity : EventDispatcher, ITouchable
{
    protected EntityType entityType = EntityType.SLIME_NONE;


    [SerializeField]
    private EntityModel entityModel;

    [HideInInspector]
    public float amplifiedSpeed;

    protected Rigidbody2D rb;
    protected Animator animator;

    [HideInInspector]
    public EntityModel model;

    protected bool ShowParticles { get; set; }
	protected bool Draggable { get; set; }
	protected bool Dragged { get; set; }
	protected bool InComboRadius { get; set; }
    protected bool IgnoreTap { get; set; } // Feature: To bypass tap delay -> smoother swipe
    protected bool ComboEnabled { get; set; }


    public HashSet<int> FingerIds { get; set; }

    protected Vector3 screenPoint;
    protected Vector3 offset;
    protected Vector3 oldPosition = Vector3.zero; // Refactor to began touch position?
    protected Vector3 futurePosition; // still needed?
    protected float lastTouchTime;

    private ParticleSystem dragParticles;
	protected ComboSystem comboSystem;
    protected GameObject player;
    protected ScoreParticleManager scoreParticleManager;
    protected EntityHelper entityHelper;

    protected override void Awake()
    {
        base.Awake();

        FingerIds = new HashSet<int>();
        model = Instantiate(entityModel);

        ShowParticles = true;
        Draggable = true;
        IgnoreTap = false;
        ComboEnabled = true;
    }

    protected virtual void Start()
    {
		comboSystem = FindObjectOfType<ComboSystem>();
        dragParticles = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        scoreParticleManager = FindObjectOfType<ScoreParticleManager>();
        entityHelper = FindObjectOfType<EntityHelper>();

        model.speed += UnityEngine.Random.Range(-model.varianceInSpeed, model.varianceInSpeed);

        amplifiedSpeed = model.speed * 60;

        InputManager.Main.Register(this);
    }

    protected virtual void Update() {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        else UpdateEntity();
    }

    protected abstract void UpdateEntity();

    public void OnTouchBegan(Touch touch)
    {
        if (GameManager.Instance.State == GameState.PAUSE) return;

        if (comboSystem.IntersectsComboCircle(Camera.main.ScreenToWorldPoint(touch.position)))
            InComboRadius = true;
        
        if (ShowParticles)
            dragParticles.Play();

        Dragged = true;
        oldPosition = transform.position;
        futurePosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));

        lastTouchTime = Time.time;
    }

    public void OnTouch(Touch touch)
    {
        if (GameManager.Instance.State == GameState.PAUSE)
            return;

        if (ShowParticles)
            dragParticles.transform.position = transform.position;

        Vector3 curScreenPoint = new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z);
        futurePosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (Draggable)
        {
            transform.position = futurePosition;

            var newVelocity = (touch.deltaPosition * touch.deltaTime) * (100 - model.weight);
            rb.velocity = newVelocity;
        }
    }

    public void OnTouchEnded(Touch touch)
    {
        if (model.health <= 0)
            return;

        Dragged = false;

        dragParticles.Stop();

        var secondsSinceTouch = Time.time - lastTouchTime;

        // If seconds since last touch is lower than X, see it as a tap
        if (!IgnoreTap && secondsSinceTouch < 0.3f)
        {
            OnTap();
        }
        else
        {
            var swipeDistance = touch.deltaPosition * touch.deltaTime;
            OnSwipe(swipeDistance);
        }
    }

    public virtual void OnTap()
    {
        Dispatch("tapped", this);
    }

    protected virtual void OnSwipe(Vector3 swipeVector)
    {
        var newVelocity = swipeVector * (100 - model.weight);
        rb.velocity = newVelocity;

        if (swipeVector.magnitude < 0.25f)
            return;

        model.health -= 1;
        if (model.health <= 0)
            Die();

        HandleCloseCallText();

        Dispatch("swiped", this);

        HandleScore();
        HandleCombo();
    }

    protected virtual void HandleCloseCallText()
    {
        if (Vector2.Distance(oldPosition, player.transform.position) < player.transform.localScale.x + 0.3f)
        {
            entityHelper.ShowCloseCallText();
        }
    }

    protected virtual void HandleScore()
    {
        if (GameManager.Instance.State == GameState.PLAY)
        {
            int addedScore = comboSystem.AwardPoints(model.awardPoints);
            if(addedScore > 0)
                scoreParticleManager.ShowRewardIndicatorAt(addedScore, transform.position, true);
        }
    }
    protected virtual void HandleScore(int addedScore)
    {
        if (GameManager.Instance.State == GameState.PLAY && addedScore > 0)
            scoreParticleManager.ShowRewardIndicatorAt(addedScore, transform.position, true);
    }

    protected virtual void HandleCombo()
    {
        if (!ComboEnabled)
            return;

        if (InComboRadius)
            comboSystem.Increase(1);
        else
            comboSystem.Decrease();
        InComboRadius = false;
    }
		
	public virtual void Accept(IVial vial) { }

	public virtual void Configure(int pointModifier){
		model.awardPoints += pointModifier;
	}
	public virtual void Configure(int pointModifier, int healthModifier){
		Configure (pointModifier);
		model.health += healthModifier;
	}
	public virtual void Configure(int pointModifier, int healthModifier, float speedModifier){
		Configure (pointModifier, healthModifier);
		model.speed += speedModifier;
	}
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        Player player = coll.gameObject.GetComponent<Player>();
        if (player)
            OnPlayerHit(player);
    }
    public void OnEntityDestroy() {
        dragParticles.Stop();
        InputManager.Main.Deregister(this);

        Destroy(gameObject);
    }
    public virtual void OnPlayerHit(Player player) {
        Dispatch("dying", this);
        OnEntityDestroy();
    }
    public void Die() {
        TrackDeath();
        Destroy(GetComponent <CircleCollider2D>() );
        StartCoroutine(DieAnimation());
    }
    protected virtual void TrackDeath() {
        if (GameManager.Instance.SaveGame.EnemyTypes.Contains(entityType))
        {
            GameManager.Instance.SaveGame.EnemyKillCount[GameManager.Instance.SaveGame.EnemyTypes.IndexOf(entityType)]++;
        }
        else
        {
            GameManager.Instance.SaveGame.EnemyTypes.Add(entityType);
            GameManager.Instance.SaveGame.EnemyKillCount.Add(1);
        }
        Debug.Log(entityType + GameManager.Instance.SaveGame.EnemyKillCount[GameManager.Instance.SaveGame.EnemyTypes.IndexOf(entityType)]);
        GameManager.Instance.Save();
    }
    private IEnumerator DieAnimation()
    {
        var shrinkStep = 0.05f;
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * shrinkStep;
            yield return new WaitForEndOfFrame();
        }

        Dispatch("dying", this);

        OnEntityDestroy();
    }

}

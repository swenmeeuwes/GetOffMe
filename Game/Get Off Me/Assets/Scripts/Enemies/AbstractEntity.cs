using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleSystem))]
public abstract class AbstractEntity : AbstractDraggable
{
    protected EntityType? entityType = null;


    [SerializeField]
    private EntityModel entityModel;

    [HideInInspector]
    public float amplifiedSpeed;
    
    protected Animator animator;

    [HideInInspector]
    public EntityModel model;

    protected bool ShowParticles { get; set; }
	protected bool Draggable { get; set; }
	protected bool Dragged { get; set; }
	protected bool InComboRadius { get; set; }
    protected bool ComboEnabled { get; set; }

    private ParticleSystem dragParticles;
	protected ComboSystem comboSystem;
    protected Rigidbody2D rbody;
    protected GameObject player;
    protected ScoreParticleManager scoreParticleManager;
    protected EntityHelper entityHelper;

    protected override void Awake()
    {
        base.Awake();

        model = Instantiate(entityModel);

        ShowParticles = true;
        Draggable = true;
        ComboEnabled = true;
        weight = model.weight;
    }

    protected virtual void Start()
    {
		comboSystem = FindObjectOfType<ComboSystem>();
        rbody = GetComponent<Rigidbody2D>();
        dragParticles = GetComponent<ParticleSystem>();        
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        scoreParticleManager = FindObjectOfType<ScoreParticleManager>();
        entityHelper = FindObjectOfType<EntityHelper>();

        model.speed += UnityEngine.Random.Range(-model.varianceInSpeed, model.varianceInSpeed);

        amplifiedSpeed = model.speed * 60;
    }

    protected virtual void Update() {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        else UpdateEntity();
    }

    protected abstract void UpdateEntity();

    public override void OnTouchBegan(Touch touch)
    {
        if (GameManager.Instance.State == GameState.PAUSE) return;

        if (comboSystem.IntersectsComboCircle(Camera.main.ScreenToWorldPoint(touch.position)))
            InComboRadius = true;
        
        if (ShowParticles)
            dragParticles.Play();

        Dragged = true;
        base.OnTouchBegan(touch);
    }

    public override void OnTouch(Touch touch)
    {
        if (GameManager.Instance.State == GameState.PAUSE)
            return;

        if (ShowParticles)
            dragParticles.transform.position = transform.position;

        if (Draggable)
        {
            base.OnTouch(touch);

            var newVelocity = (touch.deltaPosition * touch.deltaTime) * (100 - weight);
            rbody.velocity = newVelocity;
        }
    }

    public override void OnTouchEnded(Touch touch)
    {
        if (model.health <= 0)
            return;

        Dragged = false;

        dragParticles.Stop();

        base.OnTouchEnded(touch);
    }

    protected override void OnTap()
    {
        Dispatch("tapped", this);
    }

    protected override void OnSwipe(Vector3 swipeVector)
    {
        var newVelocity = swipeVector * (100 - model.weight);
        rbody.velocity = newVelocity;

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
        if (Vector2.Distance(beganTouchPosition, player.transform.position) < player.transform.localScale.x + 0.3f)
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

        Destroy(gameObject);
    }
    public virtual void OnPlayerHit(Player player) {
        Dispatch("dying", this);
        OnEntityDestroy();
    }
    public void Die() {
        player.GetComponent<Player>().enemiesKilledWithoutGettingHit++;
        TrackDeath();
        Destroy(GetComponent <CircleCollider2D>() );
        StartCoroutine(DieAnimation());
    }
    protected virtual void TrackDeath() {
        GameManager.Instance.SaveGame.EnemyKillCount[(int)entityType]++;
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

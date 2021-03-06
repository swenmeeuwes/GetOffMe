﻿using System;
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
    public EntityModel Model { get; set; }

    protected bool ShowParticles { get; set; }
	protected bool Dragged { get; set; }
	protected bool InComboRadius { get; set; }
    protected bool ComboEnabled { get; set; }

    private ParticleSystem dragParticles;
    protected Rigidbody2D rbody;
    protected GameObject player;
    protected EntityHelper entityHelper;

    protected float weight;

    protected override void Awake()
    {
        base.Awake();

        Model = Instantiate(entityModel);

        ShowParticles = true;
        Draggable = true;
        ComboEnabled = true;
        weight = Model.weight;
    }

    protected virtual void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        dragParticles = GetComponent<ParticleSystem>();        
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        entityHelper = FindObjectOfType<EntityHelper>();

        Model.speed += UnityEngine.Random.Range(-Model.varianceInSpeed, Model.varianceInSpeed);

        amplifiedSpeed = Model.speed * 60;
    }

    protected virtual void Update() {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        else UpdateEntity();
    }

    protected abstract void UpdateEntity();

    public override void OnTouchBegan(Touch touch)
    {
        if (GameManager.Instance.State == GameState.PAUSE) return;

        if (ComboSystem.Instance.IntersectsComboCircle(Camera.main.ScreenToWorldPoint(touch.position)))
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

        if (dragParticles != null && ShowParticles)
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
        if (Model.health <= 0)
            return;

        Dragged = false;

        if(dragParticles != null)
            dragParticles.Stop();

        base.OnTouchEnded(touch);
    }

    public void ApplySwipeVelocity(Vector3 swipeVector)
    {
        var newVelocity = swipeVector * (100 - Model.weight);
        rbody.velocity = newVelocity;
    }

    protected override void OnTap()
    {
        Dispatch("tapped", this);
    }

    protected override void OnSwipe(Vector3 swipeVector)
    {
        ApplySwipeVelocity(swipeVector);

        if (swipeVector.magnitude < 0.25f)
            return;

        Model.health -= 1;
        if (Model.health <= 0)
            Die();

        HandleCloseCallText();

        Dispatch("swiped", this);
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
            int addedScore = ComboSystem.Instance.AwardPoints(Model.awardPoints);
            if(addedScore > 0)
                ScoreParticleManager.Instance.ShowRewardIndicatorAt(addedScore, transform.position, true);
        }
    }
    protected virtual void HandleScore(int addedScore)
    {
        if (GameManager.Instance.State == GameState.PLAY && addedScore > 0)
            ScoreParticleManager.Instance.ShowRewardIndicatorAt(addedScore, transform.position, true);
    }

    protected virtual void HandleCombo()
    {
        if (!ComboEnabled)
            return;

        if (InComboRadius)
            ComboSystem.Instance.Increase(1);
        else
            ComboSystem.Instance.Decrease();
        InComboRadius = false;
    }
		
	public virtual void Accept(IVial vial) { }

	public virtual void Configure(int pointModifier){
		Model.awardPoints += pointModifier;
	}
	public virtual void Configure(int pointModifier, int healthModifier){
		Configure (pointModifier);
		Model.health += healthModifier;
	}
	public virtual void Configure(int pointModifier, int healthModifier, float speedModifier){
		Configure (pointModifier, healthModifier);
		Model.speed += speedModifier;
	}
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        Player player = coll.gameObject.GetComponent<Player>();
        if (player)
            OnPlayerHit(player);
    }
    public void OnEntityDestroy() {
        dragParticles.Stop();
        InputManager.Instance.Deregister(this);
        Destroy(gameObject);
    }
    public virtual void OnPlayerHit(Player player) {
        Dispatch("dying", this);
        OnEntityDestroy();
    }
    public void Die(bool handleScore = true, bool handleCombo = true) {
        if (handleScore)
            HandleScore();

        if (handleCombo)
            HandleCombo();

        SoundManager.Instance.PlaySound(SFXType.ENEMY_DEATH);
        player.GetComponent<Player>().enemiesKilledWithoutGettingHit++;
        TrackDeath();
        gameObject.layer = LayerMask.NameToLayer("Graveyard");
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

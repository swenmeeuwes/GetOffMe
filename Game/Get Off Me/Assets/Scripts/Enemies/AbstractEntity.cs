﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEntity : EventDispatcher
{
    private readonly float SWIPE_MAGNITUDE = 0.2f; // Swipe threshold, the minimum required distance for a swipe (in units)

    [SerializeField]
    private EntityModel entityModel;

    protected Rigidbody2D rb;
    protected Animator animator;

    [HideInInspector]
    public EntityModel model;

    protected Vector3 screenPoint;
    protected Vector3 offset;
    protected Vector3 oldPosition = Vector3.zero;
    protected Vector3 futurePosition;

    private ParticleSystem DragParticles;

    protected virtual void Awake()
    {
        base.Awake();

        model = Instantiate(entityModel);
    }

    protected virtual void Start()
    {
        DragParticles = GameObject.Find("EntityDragParticle").GetComponent<ParticleSystem>();
        DragParticles.Stop();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        model.speed += UnityEngine.Random.Range(-model.varianceInSpeed, model.varianceInSpeed);
    }

    private void Update() { }

    protected virtual void OnMouseDown()
    {
        DragParticles.transform.position = transform.position;
        DragParticles.Play();

        oldPosition = transform.position;
        futurePosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    protected virtual void OnMouseDrag()
    {
        DragParticles.transform.position = transform.position;
        oldPosition = transform.position;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        futurePosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        transform.position = futurePosition;
    }
    protected virtual void OnMouseUp()
    {
        DragParticles.Stop();
        var swipeVector = futurePosition - oldPosition; // Swipe distance in units

        if (swipeVector.magnitude > SWIPE_MAGNITUDE)
            OnSwipe(swipeVector);
        else
            OnTap();        
    }

    public virtual void OnTap()
    {
        Dispatch("tapped", this);
    }

    protected virtual void OnSwipe(Vector3 swipeVector)
    {
        var newVelocity = swipeVector * (100 - model.weight);
        rb.velocity = newVelocity;

        model.health -= 1;
        if (model.health <= 0)
            StartCoroutine(Die());

        Dispatch("swiped", this);
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Player player = coll.gameObject.GetComponent<Player>();
        if (player)
            OnPlayerHit(player);
    }
    public virtual void OnDestroy() {
        DragParticles.Stop();
        Destroy(gameObject);
    }
    public abstract void OnPlayerHit(Player player);
    IEnumerator Die()
    {
        var shrinkStep = 0.05f;
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * shrinkStep;
            yield return new WaitForEndOfFrame();
        }

        FindObjectOfType<ScoreParticleManager>().ShowRewardIndicatorAt(1, transform.position, true);

        ScoreManager.Instance.Score++;

        Dispatch("dying", this);

        Destroy(gameObject);
    }
}

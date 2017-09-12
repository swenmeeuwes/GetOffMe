using System;
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

    public bool ShowParticles { get; set; }
    public bool Draggable { get; set; }
    public bool Dragged { get; set; }

    protected Vector3 screenPoint;
    protected Vector3 offset;
    protected Vector3 oldPosition = Vector3.zero;
    protected Vector3 futurePosition;

    private ParticleSystem particleSystem;

    protected virtual void Awake()
    {
        base.Awake();
        model = Instantiate(entityModel);
    }

    protected virtual void Start()
    {
        ShowParticles = true;
        Draggable = true;
        particleSystem = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        model.speed += UnityEngine.Random.Range(-model.varianceInSpeed, model.varianceInSpeed);
    }

    protected virtual void Update() {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        else UpdateEntity();
    }

    protected abstract void UpdateEntity();

    protected virtual void OnMouseDown()
    {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        if (ShowParticles) {
            //particleSystem.transform.position = transform.position;
            particleSystem.Play();
        }

        Dragged = true;
        oldPosition = transform.position;
        futurePosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    protected virtual void OnMouseDrag()
    {
        if (GameManager.Instance.State == GameState.PAUSE) return;
        if (ShowParticles) {
            particleSystem.transform.position = transform.position;
        }
        
        oldPosition = transform.position;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        futurePosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (Draggable) {
            transform.position = futurePosition;
        }
    }
    protected virtual void OnMouseUp()
    {
        particleSystem.Stop();
        Dragged = false;
        if (GameManager.Instance.State == GameState.PAUSE) return;
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

        if (GameManager.Instance.State == GameState.PLAY)
        {
            ScoreManager.Instance.Score++;
            FindObjectOfType<ScoreParticleManager>().ShowRewardIndicatorAt(1, transform.position, true);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Player player = coll.gameObject.GetComponent<Player>();
        if (player)
            OnPlayerHit(player);
    }
    public virtual void OnEntityDestroy() {
        particleSystem.Stop();
        Destroy(gameObject);
    }
    public virtual void OnPlayerHit(Player player) {
        Dispatch("dying", this);
        OnEntityDestroy();
    }
    IEnumerator Die()
    {
        var shrinkStep = 0.05f;
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * shrinkStep;
            yield return new WaitForEndOfFrame();
        }

        Dispatch("dying", this);

        Destroy(gameObject);
    }
}

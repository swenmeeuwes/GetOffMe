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
    protected Transform helmet;

    public EntityModel model;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 oldPosition = Vector3.one;
    private Vector3 futurePosition;

    protected virtual void Awake()
    {
        base.Awake();

        model = Instantiate(entityModel);
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        helmet = transform.Find("Helmet");

        model.speed += UnityEngine.Random.Range(-model.varianceInSpeed, model.varianceInSpeed);

        if (helmet != null && !model.hasHelmet)
            helmet.gameObject.SetActive(false);
    }

    private void Update() { }

    private void OnMouseDown()
    {
        oldPosition = transform.position;
        futurePosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        oldPosition = transform.position;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        futurePosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (model.hasHelmet)
            return;

        transform.position = futurePosition;
    }
    private void OnMouseUp()
    {
        var swipeVector = futurePosition - oldPosition; // Swipe distance in units

        if (swipeVector.magnitude > SWIPE_MAGNITUDE)
            OnSwipe(swipeVector);
        else
            OnTap();        
    }

    private void OnTap()
    {
        if (model.hasHelmet)
        {
            model.hasHelmet = false;

            if (helmet != null)
            {
                helmet.gameObject.GetComponent<Animator>().SetTrigger("FlipOff");
                //helmet.gameObject.SetActive(false);
            }
        }

        Dispatch("tapped", this);
    }

    private void OnSwipe(Vector3 swipeVector)
    {
        if (model.hasHelmet)
            return;

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
        if (player) OnPlayerHit(player); 
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

        ScoreManager.Instance.Score++;

        Dispatch("dying", this);

        Destroy(gameObject);
    }
}

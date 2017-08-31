﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : MonoBehaviour
{
    [SerializeField]
    private Enemy enemyModel;

    protected Enemy model;
    protected GameObject target;
    protected Vector3 velocity;
    protected float acceleration; // per second
    protected float frictionAcceleration;
    protected int weight;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 oldPosition = Vector3.one;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        model = Instantiate(enemyModel);
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        oldPosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        oldPosition = transform.position;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
    void OnMouseUp()
    {
        rb.velocity = (transform.position - oldPosition) * 50;
        model.health -= 1;
        if (model.health <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        var shrinkStep = 0.05f;
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * shrinkStep;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}

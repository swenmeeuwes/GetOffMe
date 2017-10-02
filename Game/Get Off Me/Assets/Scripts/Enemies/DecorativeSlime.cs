using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] // Used in animator
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class DecorativeSlime : AbstractDraggable
{
    public Vector3 velocity;    

    [SerializeField]
    private RuntimeAnimatorController[] possibleAnimators;

    private Animator animator;
    private Rigidbody2D rbody;

    private float outOfScreenTime = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();

        animator.runtimeAnimatorController = possibleAnimators.RandomItem();

        IgnoreTap = true;
    }

    private void Update()
    {
        if (outOfScreenTime > 5f)
            DestroySelf();

        if (!ScreenUtil.WorldPositionIsInView(transform.position))
            outOfScreenTime += Time.deltaTime;

        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void DestroySelf()
    {
        InputManager.Instance.Deregister(this);
        Destroy(gameObject);
    }

    protected override void OnSwipe(Vector3 swipeVector)
    {
        var newVelocity = swipeVector * (100 - 60);
        rbody.velocity = newVelocity;
    }

    protected override void OnTap()
    {
        
    }
}

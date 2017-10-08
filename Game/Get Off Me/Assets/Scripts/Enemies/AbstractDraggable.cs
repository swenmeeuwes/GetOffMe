using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDraggable : MonoEventDispatcher, ITouchable
{
    public HashSet<int> FingerIds { get; set; }

    protected Vector3 screenPoint;
    protected Vector3 offset;
    protected Vector3 beganTouchPosition = Vector3.zero; // Refactor to began touch position?
    protected float lastTouchTime;

    public bool IgnoreTap { get; protected set; } // Feature: To bypass tap delay -> smoother swipe
    public bool Draggable { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        FingerIds = new HashSet<int>();
        IgnoreTap = false;
    }

    public virtual void OnTouchBegan(Touch touch)
    {
        beganTouchPosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));

        lastTouchTime = Time.time;
    }

    public virtual void OnTouch(Touch touch)
    {
        Vector3 curScreenPoint = new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    }

    public virtual void OnTouchEnded(Touch touch)
    {
        var secondsSinceTouch = Time.time - lastTouchTime;

        // If seconds since last touch is lower than X, see it as a tap
        if (!IgnoreTap && secondsSinceTouch < 0.3f)
        {
            OnTap();
        }
        else if (Draggable)
        {
            var swipeDistance = touch.deltaPosition * touch.deltaTime;
            OnSwipe(swipeDistance);
        }
    }

    protected abstract void OnTap();

    protected abstract void OnSwipe(Vector3 swipeVector);
}

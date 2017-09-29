﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour {
    // Inspector variables
    [SerializeField][Tooltip("The radius of the circle scanned around the touches")]
    private float castSphereRadius = 0.2f;
    // ---
#if UNITY_EDITOR
    private Vector3 previousMousePosition;
#endif

    public static InputManager Main {
        get {
            return GameObject.FindGameObjectWithTag("MainTouchManager").GetComponent<InputManager>();
        }
    }

    private List<ITouchable> registeredTouchables;

    public void Register(ITouchable touchable)
    {
        registeredTouchables.Add(touchable);
    }

    public void Deregister(ITouchable touchable)
    {
        registeredTouchables.Remove(touchable);
    }

    private void Awake()
    {
        registeredTouchables = new List<ITouchable>();

#if UNITY_EDITOR
        previousMousePosition = Input.mousePosition;
#endif
    }

    private void Update () {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            HandleTouch(touch);

            var worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            var hits = Physics2D.CircleCastAll(worldPosition, castSphereRadius, Vector3.forward);
            for (int j = 0; j < hits.Length; j++)
            {
                var hit = hits[j];
                HandleTouchOn(hit.transform, touch);                
            }
        }

#if UNITY_EDITOR
        // Mouse simulation - Wish: Make adapters, this is hard because touch and mouse have different interfaces
        var fakeTouch = new Touch()
        {
            fingerId = 99,
            position = Input.mousePosition,
            deltaPosition = Input.mousePosition - previousMousePosition,
            deltaTime = Time.deltaTime
        };

        var handleMouse = false; // SO UGLY WHYYY
        if (Input.GetMouseButtonDown(0))
        {
            fakeTouch.phase = TouchPhase.Began;
            handleMouse = true;
        }
        else if (Input.GetMouseButton(0))
        {
            fakeTouch.phase = TouchPhase.Moved;
            handleMouse = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fakeTouch.phase = TouchPhase.Ended;
            handleMouse = true;
        }

        if (handleMouse)
        {
            HandleTouch(fakeTouch);

            var fakeWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var fakeHits = Physics2D.CircleCastAll(fakeWorldPosition, castSphereRadius, Vector3.forward);
            for (int j = 0; j < fakeHits.Length; j++)
            {
                var fakeHit = fakeHits[j];
                HandleTouchOn(fakeHit.transform, fakeTouch);
            }
        }

        previousMousePosition = Input.mousePosition;
#endif
    }

    private void HandleTouchOn(Transform transform, Touch touch)
    {
        if (transform == null)
            return;

        var touched = transform.gameObject;

        if (touch.phase == TouchPhase.Began)
        {
            var touchable = touched.GetComponent<ITouchable>();
            if (touchable == null)
                return;

            // Tag the ITouchable with the fingerId so we can call OnTouch and OnTouchEnded on the same object later
            touchable.FingerIds.Add(touch.fingerId);
            registeredTouchables.Add(touchable);

            touchable.OnTouchBegan(touch);
        }
    }

    private void HandleTouch(Touch touch)
    {
        var toBeDeleted = new List<ITouchable>();
        foreach (var touchable in registeredTouchables)
        {
            if (touchable.FingerIds.Count == 0 || !touchable.FingerIds.Contains(touch.fingerId))
                continue;

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    touchable.OnTouch(touch);
                    break;
                case TouchPhase.Ended:
                    touchable.OnTouchEnded(touch);

                    // Remove tag so that the events won't fire anymore
                    touchable.FingerIds.Remove(touch.fingerId);
                    toBeDeleted.Add(touchable);
                    break;
            }
        }

        toBeDeleted.ForEach(touchable => registeredTouchables.Remove(touchable));
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var worldPosition = Camera.main.ScreenToWorldPoint(touch.position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldPosition, castSphereRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(worldPosition, touch.radius);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(worldPosition + new Vector3(0, -0.3f, 0), touch.fingerId.ToString());
#endif
        }
    }
}

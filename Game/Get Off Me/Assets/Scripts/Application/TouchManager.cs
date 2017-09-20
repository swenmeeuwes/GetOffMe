using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    // Inspector variables
    [SerializeField][Tooltip("The radius of the circle scanned around the touches")]
    private float castSphereRadius = 0.2f;

    // WISH: MOUSE CLICK TO TOUCH SIMULATION
    //[SerializeField][Tooltip("Allows mouse clicks to count as touch")]
    //private bool listenToMouse = false;
    // ---

    public static TouchManager Main {
        get {
            return GameObject.FindGameObjectWithTag("MainTouchManager").GetComponent<TouchManager>();
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
            touchable.FingerId = touch.fingerId;

            touchable.OnTouchBegan(touch);
        }
    }

    private void HandleTouch(Touch touch)
    {
        foreach (var touchable in registeredTouchables)
        {
            if (touchable.FingerId == null)
                continue;

            switch(touch.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    touchable.OnTouch(touch);
                    break;
                case TouchPhase.Ended:
                    touchable.OnTouchEnded(touch);

                    // Remove tag so that the events won't fire anymore
                    if(touchable.FingerId == touch.fingerId)
                        touchable.FingerId = null;
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var worldPosition = Camera.main.ScreenToWorldPoint(touch.position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldPosition, castSphereRadius);
        }
    }
}

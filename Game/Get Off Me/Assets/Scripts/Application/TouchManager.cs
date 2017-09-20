using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    [SerializeField][Tooltip("The radius of the circle scanned around the touches.")]
    private float castSphereRadius = 0.2f;

	private void Update () {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var worldPosition = Camera.main.ScreenToWorldPoint(touch.position);

            var hits = Physics2D.CircleCastAll(worldPosition, castSphereRadius, Vector3.forward);
            for (int j = 0; j < hits.Length; j++)
            {
                var hit = hits[j];
                HandleTouch(touch, hit.transform);
            }
        }
	}

    private void HandleTouch(Touch touch, Transform transform)
    {
        if (transform == null)
            return;
        
        switch(touch.phase)
        {
            case TouchPhase.Began:
                transform.gameObject.SendMessage("OnTouchBegan", touch, SendMessageOptions.DontRequireReceiver);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                transform.gameObject.SendMessage("OnTouch", touch, SendMessageOptions.DontRequireReceiver);
                break;
            case TouchPhase.Ended:
                transform.gameObject.SendMessage("OnTouchEnded", touch, SendMessageOptions.DontRequireReceiver);
                break;
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

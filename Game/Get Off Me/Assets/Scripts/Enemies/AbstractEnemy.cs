using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : MonoBehaviour {

    protected int health;
    protected float maxSpeed;
    protected GameObject target;
    protected Vector3 velocity;
    protected float acceleration; // per second

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 oldPosition = new Vector3(0,0,0);

    void Start() { }

    void Update() { }


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
    void OnMouseUp() {
        velocity = transform.position - oldPosition;
    }
}

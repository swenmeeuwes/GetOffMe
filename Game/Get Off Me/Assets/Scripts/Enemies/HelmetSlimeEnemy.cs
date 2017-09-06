using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetSlimeEnemy : SeekingEntity {

    bool hasHelmet;

    protected override void Start() {
        base.Start();
        hasHelmet = true;
    }
    public override void OnTap() {
        Debug.Log("tap");
        if (hasHelmet)
        {
            hasHelmet = false;
            animator.SetTrigger("loseHelmet");

            // Create flipped particle
            var helmetPrefab = Resources.Load<GameObject>("Enemy/Props/Helmet");
            var helmetObject = Instantiate(helmetPrefab);

            helmetObject.transform.position = transform.position;
            helmetObject.transform.SetParent(transform);
        }
        base.OnTap();
    }
    protected override void OnMouseDown()
    {
        if (hasHelmet)
        {
            oldPosition = transform.position;
            futurePosition = transform.position;
        }
        else {
            base.OnMouseDown();
        }
    }
    protected override void OnSwipe(Vector3 swipeVector) {
        if (hasHelmet)
            return;
        base.OnSwipe(swipeVector);
    }
    protected override void OnMouseDrag() {
        if (hasHelmet)
        {
            oldPosition = transform.position;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            futurePosition = Camera.main.ScreenToWorldPoint(curScreenPoint + offset);
        }
        else {
            base.OnMouseDrag();
        }
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health + (hasHelmet?1: 0)); // TODO Temporary extra health for helmet
        Dispatch("dying", this);
        OnDestroy();
    }
}

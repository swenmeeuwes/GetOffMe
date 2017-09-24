using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    // Use this for initialization
    void OnCollisionEnter2D(Collision2D coll)
    {
        AbstractEntity entity = coll.gameObject.GetComponent<AbstractEntity>();
        if (entity)
            entity.Die();
    }
}

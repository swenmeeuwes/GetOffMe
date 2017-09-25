﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should this be in the util package?
[RequireComponent(typeof(Animator))]
public class ExplosionScript : MonoBehaviour {
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(SlowExplodeCoroutine());
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        AbstractEntity entity = coll.gameObject.GetComponent<AbstractEntity>();
        if (entity)
            entity.Die();
    }

    private void OnDestroy()
    {
        Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
        Time.timeScale = 1;
    }

    private IEnumerator SlowExplodeCoroutine()
    {
        var nextShakeStep = Random.value;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, nextShakeStep);
            Time.timeScale = Mathf.Abs(nextShakeStep);

            nextShakeStep = -Random.value;

            yield return new WaitForEndOfFrame();
        }

        Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
        Time.timeScale = 1;

        Destroy(gameObject);
    }
}

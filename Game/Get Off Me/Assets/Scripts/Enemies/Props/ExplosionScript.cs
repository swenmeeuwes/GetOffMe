using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplosionScript : MonoBehaviour {
    private Animator animator;
    [SerializeField]
    private int vibrationDuration = 800;

    private void Start()
    {
        animator = GetComponent<Animator>();

        VibrationService.Vibrate(vibrationDuration);

        if (PlayerPrefs.GetInt(PlayerPrefsLiterals.CAMERA_SHAKE.ToString(), 0) == 1)
            StartCoroutine(SlowExplodeCoroutine());
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        AbstractEntity entity = coll.gameObject.GetComponent<AbstractEntity>();
        if (entity) {
            int score = ComboSystem.Instance.AwardPoints(entity.model.awardPoints);
            if (score > 0) {
                ScoreParticleManager.Instance.ShowRewardIndicatorAt(score, entity.transform.position, true);
            }
            entity.Die();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    public AnimationCurve sizeInterpolation;
    public float health;
    public int absorbPercentage;

    private Camera orthographicCamera;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float startScale;

    private float maxHealth;
    private float targetSize;

    private void Start()
    {
        orthographicCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        maxHealth = health;

        startScale = transform.localScale.x; // ASSUMPTION: Player is a square
    }

    public void AbsorbEnemy(float size) {
        float damageAmount = size / Mathf.Clamp((100 - absorbPercentage), 1, 100);
        Damage(damageAmount);
    }

    private void Damage(float amount)
    {
        health -= amount;
        UpdateSize();
    }

    private void UpdateSize()
    {
        var playerTextureHeight = spriteRenderer.sprite.texture.height;
        var maxScale = 15.6f; // Size in units to touch the top of the camera, we could compute this dynamically...

        var lerpPosition = sizeInterpolation.Evaluate(1 - health / maxHealth);
        var newSize = Mathf.Lerp(startScale, maxScale, lerpPosition);

        targetSize = newSize;

        animator.SetTrigger("hit");
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        var growStep = 0.1f;
        while (transform.localScale.x < targetSize)
        {
            transform.localScale += Vector3.one * growStep;
            yield return new WaitForEndOfFrame();
        }

        // Temp death function
        if (health <= 0)
            GameManager.Instance.State = GameState.GAMEOVER;
    }
}

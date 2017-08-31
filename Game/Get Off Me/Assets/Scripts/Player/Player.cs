using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    public AnimationCurve sizeInterpolation;
    public float health;
    public int absorbPercentage;

    private Camera orthographicCamera;
    private SpriteRenderer spriteRenderer;

    private float maxHealth;
    private float targetSize;

    private void Start()
    {
        maxHealth = health;
        orthographicCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnemyEnter(float size) {
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
        var startScale = 1f; // Maybe editor tool for this?
        var maxScale = 15.6f; // Size in units to touch the top of the camera, maybe we could compute this dynamically...

        var lerpPosition = sizeInterpolation.Evaluate(1 - health / maxHealth);
        var newSize = Mathf.Lerp(startScale, maxScale, lerpPosition);

        targetSize = newSize;

        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        var growStep = 0.1f;
        while (transform.localScale.x < targetSize)
        {
            transform.localScale += Vector3.one * growStep;
            yield return new WaitForEndOfFrame();
        }

        // Temp death function
        if (health <= 0)
            SceneManager.LoadScene("StartMenu");
    }
}

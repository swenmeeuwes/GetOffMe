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

    private void Start()
    {
        maxHealth = health;
        orthographicCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnemyEnter(float size) {
        float damageAmount = size / (100 - absorbPercentage);
        Damage(damageAmount);
    }

    private void Damage(float amount)
    {
        health -= amount;
        UpdateSize();

        // Temp death function
        if (health <= 0)
            SceneManager.LoadScene("StartMenu");
    }

    private void UpdateSize()
    {
        var playerTextureHeight = spriteRenderer.sprite.texture.height;
        var startScale = 1f; // Maybe editor tool for this?
        var maxScale = 15.6f; // Size in units to touch the top of the camera, maybe we could compute this dynamically...

        var lerpPosition = sizeInterpolation.Evaluate(1 - health / maxHealth);
        var newSize = Mathf.Lerp(startScale, maxScale, lerpPosition);

        transform.localScale = new Vector2(newSize, newSize);
    }
}

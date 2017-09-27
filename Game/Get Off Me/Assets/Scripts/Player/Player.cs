using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    public AnimationCurve sizeInterpolation;
    public float health;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
	private ComboSystem comboSystem;

    private float startScale;

    public float maxHealth;
    private float targetSize;

    private void Awake() {
        maxHealth = health;
        comboSystem = GameObject.Find("ComboSystem").GetComponent<ComboSystem>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        startScale = transform.localScale.x; // ASSUMPTION: Player is a square
    }

    public void AbsorbEnemy(float size) {
        Damage(1);
    }

    public void PauseButton() {
        if (GameManager.Instance.State == GameState.GAMEOVER) return;
        if (GameManager.Instance.State == GameState.PAUSE)
            GameManager.Instance.Resume();
        else
            GameManager.Instance.Pause();
    }
    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, -1, maxHealth);
        UpdateSize();
    }

    public void Damage(float amount)
    {
		comboSystem.Decrease();
        health -= amount;

        VibrationService.Vibrate(800);
        animator.SetTrigger("hit");
        UpdateSize();
    }

    private void UpdateSize()
    {
        var playerTextureHeight = spriteRenderer.sprite.texture.height;
        var maxScale = 15.6f; // Size in units to touch the top of the camera, we could compute this dynamically...

        var lerpPosition = sizeInterpolation.Evaluate(1 - health / maxHealth);
        var newSize = Mathf.Lerp(startScale, maxScale, lerpPosition);

        targetSize = newSize;

        StartCoroutine(AdaptSize());
    }
    public void GameOver() {
        GameObject.Find("Spawner").GetComponent<OffScreenSpawner>().DestroyAllSpawns();
        GameManager.Instance.State = GameState.GAMEOVER;
    }
    private IEnumerator AdaptSize()
    {
        var step = 0.2f;

        if (transform.localScale.x < targetSize)
        {
            while (transform.localScale.x < targetSize)
            {
                transform.localScale += Vector3.one * step;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (transform.localScale.x > targetSize)
            {
                transform.localScale -= Vector3.one * step;
                yield return new WaitForEndOfFrame();
            }
        }

        if (health <= 0)
            GameOver();
    }
}

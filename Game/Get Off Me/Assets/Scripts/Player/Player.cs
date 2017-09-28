using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    public AnimationCurve sizeInterpolation;
    public float health;

    [SerializeField]
    public int onHitVibrationDuration = 600;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
	private ComboSystem comboSystem;
    private GameObject onFireObject;
    private ParticleSystem healParticles;

    private float startScale;

    public float maxHealth;
    private float targetSize;

    [HideInInspector]
    public int enemiesKilledWithoutGettingHit = 0;

    private bool m_Lit;
    public bool Lit {
        get {
            return m_Lit;
        }
        set {
            m_Lit = value;
            onFireObject.SetActive(m_Lit);
        }
    }

    private void Awake() {
        maxHealth = health;
        comboSystem = GameObject.Find("ComboSystem").GetComponent<ComboSystem>();
        onFireObject = GameObject.Find("OnFire");
        healParticles = GameObject.Find("HealParticles").GetComponent<ParticleSystem>();

        onFireObject.SetActive(false);
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
        HandleHealParticles();
        health = Mathf.Clamp(health + amount, -1, maxHealth);
        UpdateSize();
    }
    private void HandleHealParticles() {
        StopCoroutine("EmitHealParticles");
        healParticles.Stop();
        StartCoroutine(EmitHealParticles(1.0f));
    }
    private IEnumerator EmitHealParticles(float seconds)
    {
        float counter = 0;
        healParticles.Play();
        while (counter < seconds)
        {
            counter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        healParticles.Stop();
    }
    public void Damage(float amount)
    {
        GameManager.Instance.HandleEnemiesKilledWithoutGettingHit(enemiesKilledWithoutGettingHit);
        enemiesKilledWithoutGettingHit = 0;
        comboSystem.Decrease();
        health -= amount;

        VibrationService.Vibrate(onHitVibrationDuration);
        animator.SetTrigger("hit");
        Camera.main.GetComponent<Animator>().Play("CameraShake", 0);
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

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    public AnimationCurve sizeInterpolation;
    public float health;

    [SerializeField]
    public int onHitVibrationDuration = 600;

    [Header("Shockwave")]
    [SerializeField]
    public float minCameraSizeOnShockwave = 4.5f;
    public float maxCameraSize = 5f;
    public float shockwaveEffectiveRange = 2f;
    public float shockwaveForce = 60f;
    public float cameraRestoreDuration = 1f;
    public float shockwaveCooldown = 2f;
    public int shockwaveChargedNeeded = 100;
    [Tooltip("How much the player will shake when it's fully charged")]
    public float shakeOffset = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject onFireObject;
    private ParticleSystem healParticles;
    private ParticleSystem shockwaveParticles;

    private float startScale;

    [HideInInspector]
    public int shockwaveCharge = 0;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public int enemiesKilledWithoutGettingHit = 0;

    private float targetSize;
    private float lastShockwaveTime = 0;
    private Vector3 anchorPosition;
    private Vector3 chargePosition;

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

    private Action<object> detectedGestureListener;
    private Action<object> comboChangedListener;

    private void Awake() {
        maxHealth = health;

        onFireObject = GameObject.Find("OnFire");
        healParticles = GameObject.Find("HealParticles").GetComponent<ParticleSystem>();
        shockwaveParticles = GameObject.Find("ShockwaveParticles").GetComponent<ParticleSystem>();

        onFireObject.SetActive(false);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        anchorPosition = transform.position;
        startScale = transform.localScale.x; // ASSUMPTION: Player is a square

        // Listeners
        detectedGestureListener = eventObject => HandleGestureDetected(eventObject);
        InputManager.Instance.AddEventListener(InputManager.GESTURE_DETECTED, detectedGestureListener);

        comboChangedListener = eventObject => HandleComboChanged((ComboChangedEvent)eventObject);
        ComboSystem.Instance.AddEventListener(ComboSystem.COMBO_CHANGED, comboChangedListener);

        // Play animations
        StartCoroutine(ShowShockwaveCharge());
    }

    private void Update()
    {
        Camera.main.orthographicSize += ((maxCameraSize - Camera.main.orthographicSize) * Time.deltaTime) / cameraRestoreDuration;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraSizeOnShockwave, 5f);
    }

    private void LateUpdate()
    {
        transform.position = chargePosition;
        spriteRenderer.color = shockwaveCharge == shockwaveChargedNeeded ? Color.blue : Color.white;
    }

    private void OnDestroy()
    {
        InputManager.Instance.RemoveEventListener(InputManager.GESTURE_DETECTED, detectedGestureListener);
        ComboSystem.Instance.RemoveEventListener(ComboSystem.COMBO_CHANGED, comboChangedListener);
    }

    public void AbsorbEnemy(float size) {
        Damage(1);
    }

    private void HandleGestureDetected(object gesture)
    {
        if (gesture is PinchGesture)
        {
            var pinchGesture = (PinchGesture)gesture;

            if (Camera.main.orthographic)
            {
                var isOnCooldown = Time.time - lastShockwaveTime < shockwaveCooldown;
                var isCharged = shockwaveCharge == shockwaveChargedNeeded;

                if (pinchGesture.DeltaMagnitude < 0)
                    Camera.main.orthographicSize += pinchGesture.DeltaMagnitude * InputManager.PINCH_GESTURE_SPEED_MODIFIER;

                if (Camera.main.orthographicSize <= minCameraSizeOnShockwave + 0.2f && !isOnCooldown && isCharged)
                    ExecuteShockwaveAbility(); // We could make an ability system, but not needed for now

                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraSizeOnShockwave, maxCameraSize);
            }
        }

        // Future gestures here ...
    }

    private void HandleComboChanged(ComboChangedEvent comboChangedEvent)
    {
        if (comboChangedEvent.ComboDelta > 0)
            shockwaveCharge += comboChangedEvent.NewCombo;

        shockwaveCharge = Mathf.Clamp(shockwaveCharge, 0, shockwaveChargedNeeded);
    }

    private void ExecuteShockwaveAbility()
    {
        if (shockwaveCharge < shockwaveChargedNeeded)
            return;

        shockwaveParticles.Emit(60);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var entityScript = enemy.GetComponent<AbstractEntity>();
            var distanceToPlayer = Vector2.Distance(enemy.transform.position, transform.position);
            var playerScale = transform.lossyScale.x * (64f / 100f);
            if (distanceToPlayer < playerScale + shockwaveEffectiveRange) // Don't change to lesser equal, as this could possibly lead to a divide by 0 exception!
            {
                var directionVector = enemy.transform.position - transform.position;
                entityScript.ApplySwipeVelocity((directionVector.normalized * shockwaveForce) / (distanceToPlayer - playerScale - shockwaveEffectiveRange));

                entityScript.Die();
            }
            else
            {
                var directionVector = enemy.transform.position - transform.position;
                entityScript.ApplySwipeVelocity((directionVector.normalized * shockwaveForce) / (distanceToPlayer - playerScale - shockwaveEffectiveRange));
            }
        }

        shockwaveCharge = 0;
        lastShockwaveTime = Time.time;
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
        ComboSystem.Instance.Decrease();
        health -= amount;

        VibrationService.Vibrate(onHitVibrationDuration);
        animator.SetTrigger("hit");
        if (PlayerPrefs.GetInt(PlayerPrefsLiterals.CAMERA_SHAKE.ToString(), 0) == 1)
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

    private IEnumerator ShowShockwaveCharge()
    {
        var shake = shakeOffset;
        while (true) //shockwaveCharge > 0
        {
            shake = -shake;
            var shakeStrengh = shockwaveCharge / (float)shockwaveChargedNeeded;

            chargePosition = anchorPosition + new Vector3(shake * shakeStrengh, 0, 0);

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x * (64f / 100f) + shockwaveEffectiveRange);
    }
}

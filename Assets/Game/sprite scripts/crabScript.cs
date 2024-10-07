using UnityEngine;
using System.Collections;
using Cinemachine;

public class crabScript : MonoBehaviour
{
    public float speed;
    public float health;
    public float attack_damage;
    public Transform heroTransform;
    public float chargeTime = 2f;
    private int points = 27;
    private KcountScript killCounter;
    private eosScript enemiesOnScreen;
    private scoreScript score;
    private SpriteRenderer spriteRenderer;
    private float originalSpeed;
    private float chargeSpeed;
    private bool isCharging = false;
    private Color originalColor;
    private CrabHTScript crabHScript;
    private float chargeTimer = 0f;
    private bool isFlashing = false;
    private bool isDead = false; // To prevent multiple death calls

    public AudioClip crabDieSound;  
    public AudioSource audioSource; 

    // Reference to the death particles
    private ParticleSystem deathParticles;
    private Color[] flashColors = new Color[3];

    private CinemachineImpulseSource impulseSource;

    public float explosionRadius = 12f; // Radius of the explosion
    public float explosionForce = 55f; // Force of the explosion

    public void Initialize(float initialSpeed, float initialDamage, float initialHealth)
    {
        speed = initialSpeed;
        attack_damage = initialDamage;
        health = initialHealth;
        originalSpeed = speed;
        chargeSpeed = Random.Range(speed * 3f, speed * 5f);
    }

    void Start()
    {
        killCounter = FindObjectOfType<KcountScript>();
        score = FindObjectOfType<scoreScript>();
        enemiesOnScreen = FindAnyObjectByType<eosScript>();
        crabHScript = GetComponentInChildren<CrabHTScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // Initialize particles and ensure they don't play at the start
        deathParticles = GetComponent<ParticleSystem>();
        if (deathParticles != null)
        {
            deathParticles.Stop();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        flashColors[0] = new Color(1f, 1f, 1f, 0.97f);
        flashColors[1] = new Color(0.95f, 0.95f, 0.95f, 0.98f);
        flashColors[2] = new Color(0.90f, 0.90f, 0.90f, 0.99f);

        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        if (hero != null)
        {
            heroTransform = hero.transform;
        }
        else
        {
            Debug.LogError("Hero not found! Ensure the hero has the correct tag.");
        }

        StartCoroutine(FlashColors());
    }

    void Update()
    {
        if (!isDead)
        {
            if (crabHScript != null)
            {
                crabHScript.MirrorHealth((int)health);
            }

            if (heroTransform != null)
            {
                MoveTowardsHero();
            }

            if (isCharging)
            {
                chargeTimer -= Time.deltaTime;
                if (chargeTimer <= 0)
                {
                    EndCharging();
                }
            }

            if (health <= 0)
            {
                Die();
            }
        }
    }

    void MoveTowardsHero()
    {
        float currentSpeed = isCharging ? chargeSpeed : speed;
        transform.position = Vector3.MoveTowards(transform.position, heroTransform.position, currentSpeed * Time.deltaTime);
    }

    public void StartCharging()
    {
        isCharging = true;
        chargeTimer = chargeTime;
        attack_damage *= 2;
        speed = chargeSpeed;
    }

    void EndCharging()
    {
        isCharging = false;
        speed = originalSpeed;
        attack_damage /= 2;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (!isFlashing && spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (health <= 0 && !isDead)
        {
            Die();
            health = 0;
        }

        AliveShake();
    }

    IEnumerator FlashRed()
    {
        isFlashing = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }

    IEnumerator FlashColors()
    {
        int index = 0;
        while (true)
        {
            if (!isFlashing)
            {
                spriteRenderer.color = flashColors[index];
                index = (index + 1) % flashColors.Length;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        PlayDestoryedSound();

        if (isDead) return;

        isDead = true;

        health = 0;

        crabHScript.MirrorHealth((int)health);

        // Stop flashing
        StopAllCoroutines();

        // Disable colliders to prevent further interactions
        DisableColliders();

        speed = 0;
        spriteRenderer.color = Color.clear;

        // Play the death particles
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        ExplodeEnemiesAway(transform.position, explosionRadius, explosionForce);

        // Make the crab invisible
        spriteRenderer.color = Color.clear;

        // Handle other death logic
        killCounter.IncreaseKillCount();
        enemiesOnScreen.Subtract();
        score.IncreaseScore(points);

        DeadShake();

        // Destroy the crab after the particle effect finishes
        Destroy(gameObject, deathParticles.main.duration);
    }

    void PlayDestoryedSound()
    {
        if (audioSource != null && crabDieSound != null)
        {
            // Ensure that the sound only plays on death and not on spawn
            audioSource.clip = crabDieSound;
            audioSource.Play();
        }
    }

void AliveShake()
{
    // Custom velocity for AliveShake impulse
    Vector3 aliveImpulseVelocity = new Vector3(-0.1f, -0.1f, 0f);
    GenerateImpulseWithCustomVelocity(aliveImpulseVelocity);
}

void DeadShake()
{
    // Custom velocity for DeadShake impulse
    Vector3 deadImpulseVelocity = new Vector3(-0.4f, -0.4f, 0f);
    GenerateImpulseWithCustomVelocity(deadImpulseVelocity);
}

void GenerateImpulseWithCustomVelocity(Vector3 customVelocity)
{
    if (impulseSource != null)
    {
        // Set the custom velocity for the impulse and generate it
        impulseSource.m_DefaultVelocity = customVelocity;
        impulseSource.GenerateImpulse();
    }
    else
    {
        Debug.LogError("CinemachineImpulseSource not found on the object.");
    }
}

void ExplodeEnemiesAway(Vector2 explosionPosition, float explosionRadius, float explosionForce)
{
    // Detect all enemies within the explosion radius
    Collider2D[] enemies = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

    foreach (Collider2D enemy in enemies)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null && enemy.gameObject != this.gameObject) // Ensure it doesn't affect itself
        {
            // Calculate the direction from the explosion to the enemy
            Vector2 direction = (rb.position - explosionPosition).normalized;

            // Apply force to the enemy's Rigidbody2D
            rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
        }
    }
}

    void DisableColliders()
    {
        // Get all Collider2D components attached to the crab and disable them
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
}








using UnityEngine;
using System.Collections;
using Cinemachine;
using System.Runtime.CompilerServices;

public class ghostScript : MonoBehaviour
{
    public float speed; // Speed of the ghost
    public float health; // Health of the ghost
    public float attack_damage; // Attack damage of the ghost
    public float breakingDistance = 1.5f; // Distance at which the ghost stops charging

    private float initialHealth; // Store the initial health to restore after charging
    private float chargeSpeed; // Speed during charging
    private bool isCharging = false;
    private float chargeTimer = 0f;
    private bool isFlashing = false; // Check if the ghost is currently flashing
    private bool isDead = false; // Prevents multiple calls to Die()

    private Transform heroTransform; // Reference to the hero's transform
    private KcountScript killCounter;
    private eosScript enemiesOnScreen;
    private scoreScript score;
    private SpriteRenderer spriteRenderer;
    private GhostHScript ghScript;

    private Color originalColor; // Store the original color
    private Coroutine chargeFlickerCoroutine;

    private ParticleSystem deathParticles; // Reference to the death particles

    private CinemachineImpulseSource impulseSource;

    [Header("Audio")]
    public AudioClip ghostDieSound;  // Sound for throwing the spear
    public AudioSource audioSource;    // AudioSource component assigned via the Inspector

    public AudioClip chargeSound;  // Sound for throwing the spear
    public AudioSource audioSource2;    // AudioSource component assigned via the Inspector

    [Header("sound proximity")]
    public float detectionRadius = 15f; // How close the ghost needs to be to play the sound

    [Header("Explosion Visual Physics")]
    public float explosionRadius = 15f; // Radius of the explosion
    public float explosionForce = 55f; // Force of the explosion

    [Header("Explosion Damage")]
    public float damageRadius = 7f; // How far the damage can reach

    public float explosionDamage = 30f; // How much damage the explosion causes

    [Header("Shake Intensity")]
    public float aliveShakeIntensity = -0.13f;
    public float deadShakeIntensity = -0.5f;



    // Initialize method to set ghost's attributes from the spawner
    public void Initialize(float initialSpeed, float initialHealth, float initialDamage)
    {
        speed = initialSpeed;
        health = initialHealth;
        initialHealth = health; // Save the initial health for restoration after charging
        attack_damage = initialDamage;

        chargeSpeed = Random.Range(speed * 1.2f, speed * 1.5f); // Generate random charge speed
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color

        // Find references
        killCounter = FindObjectOfType<KcountScript>();
        score = FindObjectOfType<scoreScript>();
        enemiesOnScreen = FindAnyObjectByType<eosScript>();
        ghScript = GetComponentInChildren<GhostHScript>();
        deathParticles = GetComponent<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // Ensure the particle system is stopped on spawn
        deathParticles.Stop();

        // Find the hero by tag
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        if (hero != null)
        {
            heroTransform = hero.transform;
        }
        else
        {
            Debug.LogError("Hero not found! Ensure your hero has the correct tag.");
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (ghScript != null)
            {
                ghScript.MirrorHealth((int)health); // Update the ghost's health UI
            }

            if (heroTransform != null)
            {
                MoveTowardsHero();
            }

            // Handle random charging logic
            int rm1 = Random.Range(1, 100);
            int rm2 = Random.Range(2, 99);
            if (rm1 == rm2 && !isCharging)
            {
                StartCharging();
            }

            // Handle charging timer and breaking distance
            if (isCharging)
            {
                float distanceToHero = Vector3.Distance(transform.position, heroTransform.position);

                // If the ghost is too close to the hero, stop charging
                if (distanceToHero <= breakingDistance)
                {
                    EndCharging();
                }

                chargeTimer -= Time.deltaTime;
                if (chargeTimer <= 0)
                {
                    EndCharging();
                }
            }

            // Check if the ghost should die
            if (health <= 0)
            {
                Die();
            }

            AffirmPauseInAudioSource2();
        }

        if (health <= 0)
        {
            spriteRenderer.color = Color.clear;
        }

       
    }

    void MoveTowardsHero()
    {
        transform.position = Vector3.MoveTowards(transform.position, heroTransform.position, speed * Time.deltaTime);
    }



    public void TakeDamage(float damageAmount)
    {
        if (!isCharging && !isFlashing)
        {
            StartCoroutine(FlashRed()); // Flash white when hit (only if not charging)
        }

        health -= damageAmount;

        if (health <= 0)
        {
            Die(); // Trigger death if health drops to zero or below
            health = 0; // Ensure health is not negative
        }

        AliveShake();
    }

    void StartCharging()
    {
        if (IsGhostNearMainCamera())  // Play sound only if near the camera/hero
        {
            isCharging = true;
            PlayChargeSound();
            speed = chargeSpeed;
            chargeTimer = 2f;
            attack_damage *= 2;

            initialHealth = health;  // Store the current health
            health = 15;

            if (chargeFlickerCoroutine == null)
            {
                chargeFlickerCoroutine = StartCoroutine(FlickerColors());
            }
        }
    }

    void EndCharging()
    {
        isCharging = false;
        speed = chargeSpeed / 3f; // Reset to normal speed after charging
        attack_damage /= 2;

        // Restore health to its previous value before charging
        health = initialHealth;

        // Stop the flicker effect and reset color
        StopFlickering();

        audioSource2.Stop();
    }

    void Die()
    {
        PlayDestoryedSound();

        if (isDead) return; // Ensure Die() is only called once

        isDead = true; // Mark the ghost as dead

        health = 0;

        ghScript.MirrorHealth((int)health); // Update the ghost's health UI

        DisableColliders();

        // Stop any flickering or charging effects
        StopFlickering();
        StopCharging();

        speed = 0; // Stop the ghost from moving
        spriteRenderer.color = Color.clear; // Make the ghost invisible

        // Play the death particles
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        ExplodeEnemiesAway(transform.position, explosionRadius, explosionForce, explosionDamage, damageRadius);

        // Handle other death logic
        killCounter.IncreaseKillCount();
        enemiesOnScreen.Subtract();
        score.IncreaseScore(50);

        DeadShake();

        // Destroy the ghost after the particle effect finishes
        Destroy(gameObject, deathParticles.main.duration);
    }

    void StopFlickering()
    {
        if (chargeFlickerCoroutine != null)
        {
            StopCoroutine(chargeFlickerCoroutine);
            chargeFlickerCoroutine = null;
        }
    }

    void StopCharging()
    {
        isCharging = false;
        speed = 0;
        audioSource2.Stop();
    }

   void PlayDestoryedSound()
    {
        if (audioSource != null && ghostDieSound != null)
        {
            // Ensure that the sound only plays on death and not on spawn
            audioSource.clip = ghostDieSound;
            audioSource.Play();
        }
    }

    void PlayChargeSound()
    {
        if (audioSource2 != null && chargeSound != null && isCharging)
        {
            // Ensure that the sound only plays on death and not on spawn
            audioSource2.clip = chargeSound;
            audioSource2.Play();
        }
       
    }

    bool IsGhostNearMainCamera()
    {
        Vector3 mainCameraPosition = Camera.main.transform.position;
        float distanceToCamera = Vector3.Distance(transform.position, mainCameraPosition);
        return distanceToCamera <= detectionRadius;
    }

    // Coroutine to flicker between red, black, and blue rapidly during charging
    IEnumerator FlickerColors()
    {
        Color orange = new Color(1f, 0.5f, 0f); // RGB values for orange
        while (isCharging)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = orange;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Coroutine to flash white briefly when hit
    IEnumerator FlashRed()
    {
        if (health > 0)
        {
            isFlashing = true;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
            isFlashing = false;
        }
        
    }

void AliveShake()
{
    // Custom velocity for AliveShake impulse
    Vector3 aliveImpulseVelocity = new Vector3(aliveShakeIntensity, aliveShakeIntensity, 0f);
    GenerateImpulseWithCustomVelocity(aliveImpulseVelocity);
}

void DeadShake()
{
    // Custom velocity for DeadShake impulse
    Vector3 deadImpulseVelocity = new Vector3(deadShakeIntensity, deadShakeIntensity, 0f);
    GenerateImpulseWithCustomVelocity(deadImpulseVelocity);
}

    void ExplodeEnemiesAway(Vector2 explosionPosition, float explosionRadius, float explosionForce, float damageRadius, float explosionDamage)
    {
        // Apply explosion force to enemies within the explosionRadius
        Collider2D[] enemiesForForce = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (Collider2D enemy in enemiesForForce)
        {
            // Ensure the enemy has a Rigidbody2D and isn't the current exploding enemy
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null && enemy.gameObject != this.gameObject)
            {
                // Apply explosion force to the enemy's Rigidbody2D
                Vector2 direction = (rb.position - explosionPosition).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }

        // Apply damage to enemies within the damageRadius
        Collider2D[] enemiesForDamage = Physics2D.OverlapCircleAll(explosionPosition, damageRadius);

        foreach (Collider2D enemy in enemiesForDamage)
        {
            if (enemy.gameObject != this.gameObject)
            {
                // Apply damage based on the type of enemy
                if (enemy.GetComponent<crabScript>() != null)
                {
                    enemy.GetComponent<crabScript>().TakeDamage(explosionDamage);
                }
                else if (enemy.GetComponent<ghostScript>() != null)
                {
                    enemy.GetComponent<ghostScript>().TakeDamage(explosionDamage);
                }
                else if (enemy.GetComponent<cyclopsScript>() != null)
                {
                    enemy.GetComponent<cyclopsScript>().TakeDamage(explosionDamage);
                }
            }
        }
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

    void DisableColliders()
    {
        // Get all Collider2D components attached to the ghost and disable them
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void AffirmPauseInAudioSource2()
    {
        if (Time.timeScale == 0)
        {
            // If the game is paused, pause all relevant sounds
            audioSource2.Pause();
        }
        else
        {
            // If the game is unpaused, resume the charging sound if the ghost is charging
            if (isCharging && !audioSource2.isPlaying)
            {
                audioSource2.UnPause();
            }
        }

    }

}










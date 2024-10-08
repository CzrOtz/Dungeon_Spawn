using UnityEngine;
using System.Collections;
using Cinemachine;

public class cyclopsScript : MonoBehaviour
{
    [Header("Cyclops Stats")]
    public float speed;           // Base speed
    public float health;          // Cyclops health
    public float attack_damage;   // Cyclops attack damage
    private int points = 122;
    public Transform heroTransform;
    private KcountScript killCounter;
    private eosScript enemiesOnScreen;
    private scoreScript score;
    private CHScript chScript;
    public float acceleration = 0.7f;
    private float currentSpeed = 0f;
    private float maxSpeed;
    private bool isDead = false;  // To prevent multiple calls to Die()

    [Header("Audio")]
    public AudioClip cyDieSound;
    public AudioSource audioSource;

    [Header("Explosion Visual Physics")]
    public float explosionRadius = 50f;  // Radius of the explosion
    public float explosionForce = 200f;  // Force of the explosion

    [Header("Explosion Damage")]
    public float damageRadius = 35f;     // How far the damage can reach
    public float explosionDamage = 100f; // How much damage the explosion causes

    // Reference to the cyclopsRenderScript
    private cyclopsRenderScript renderScript;

    // Shake effect variables
    private CinemachineImpulseSource impulseSource;

    [Header("Shake Intensity")]
    public float deadShakeIntensity = -0.7f;
    public float aliveShakeIntensity = -0.12f;

    // Death particles
    private ParticleSystem deathParticles;

    // Method to initialize cyclops values from the spawner
    public void Initialize(float initialSpeed, float initialDamage, float initialHealth)
    {
        speed = initialSpeed;
        attack_damage = initialDamage;
        health = initialHealth;

        maxSpeed = speed;
        currentSpeed = 0f; // Reset acceleration speed
    }

    void Start()
    {
        killCounter = FindAnyObjectByType<KcountScript>();
        enemiesOnScreen = FindAnyObjectByType<eosScript>();
        score = FindObjectOfType<scoreScript>();
        chScript = GetComponentInChildren<CHScript>();

        // Get the render script from the child
        renderScript = GetComponentInChildren<cyclopsRenderScript>();
        if (renderScript == null)
        {
            Debug.LogError("cyclopsRenderScript not found in child object!");
        }

        // Get CinemachineImpulseSource
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            Debug.LogError("CinemachineImpulseSource not found on cyclops!");
        }

        // Get death particles
        deathParticles = GetComponent<ParticleSystem>();
        if (deathParticles != null)
        {
            deathParticles.Stop();
        }
        else
        {
            Debug.LogError("ParticleSystem (deathParticles) not found on cyclops!");
        }

        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        if (hero != null)
        {
            heroTransform = hero.transform;
        }
        else
        {
            Debug.LogError("Hero not found! Ensure hero has the correct tag.");
        }

        if (chScript != null)
        {
            chScript.MirrorHealth((int)health);
        }

        maxSpeed = speed;
    }

    void Update()
    {
        if (!isDead && heroTransform != null)
        {
            MoveTowardsHero();
        }
    }

    void MoveTowardsHero()
    {
        float distance = Vector3.Distance(transform.position, heroTransform.position);
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        if (distance < 7f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, acceleration * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, heroTransform.position, currentSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        // Notify the render script to flash red when hit
        if (renderScript != null)
        {
            renderScript.StartCoroutine(renderScript.FlashRed());
        }

        // Trigger alive shake
        AliveShake();

        if (chScript != null)
        {
            chScript.MirrorHealth((int)health);
        }

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Ensure Die() is only called once

        isDead = true;

        // Ensure health is 0
        health = 0;
        if (chScript != null)
        {
            chScript.MirrorHealth((int)health);
        }

        // Stop movement
        speed = 0;

        // Disable colliders
        DisableColliders();

        // Play the death sound
        PlayDestroyedSound();

        // Play death particles
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        // Notify the render script to handle death visuals (e.g., stop bouncing, make invisible)
        if (renderScript != null)
        {
            renderScript.Die();
        }

        // Apply explosion effects
        ExplodeEnemiesAway(transform.position, explosionRadius, explosionForce, damageRadius, explosionDamage);

        // Trigger death shake
        DeadShake();

        // Handle other death logic
        killCounter.IncreaseKillCount();
        enemiesOnScreen.Subtract();
        score.IncreaseScore(points);

        // Destroy the cyclops after a delay (matching the death particles duration)
        Destroy(gameObject, 1.5f);
    }

    void PlayDestroyedSound()
    {
        if (audioSource != null && cyDieSound != null)
        {
            audioSource.clip = cyDieSound;
            audioSource.Play();
        }
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

    void DisableColliders()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    // Shake effects
    void AliveShake()
    {
        Vector3 aliveImpulseVelocity = new Vector3(aliveShakeIntensity, aliveShakeIntensity, 0f);
        GenerateImpulseWithCustomVelocity(aliveImpulseVelocity);
    }

    void DeadShake()
    {
        Vector3 deadImpulseVelocity = new Vector3(deadShakeIntensity, deadShakeIntensity, 0f);
        GenerateImpulseWithCustomVelocity(deadImpulseVelocity);
    }

    void GenerateImpulseWithCustomVelocity(Vector3 customVelocity)
    {
        if (impulseSource != null)
        {
            impulseSource.m_DefaultVelocity = customVelocity;
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogError("CinemachineImpulseSource not found on the object.");
        }
    }
}









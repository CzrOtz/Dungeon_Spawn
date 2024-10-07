using UnityEngine;
using System.Collections;
using Cinemachine;

public class cyclopsScript : MonoBehaviour
{
    public float speed; // Base speed
    public float health; // Cyclops health
    public float attack_damage; // Cyclops attack damage
    private int points = 122;
    public Transform heroTransform;
    private KcountScript killCounter;
    private eosScript enemiesOnScreen;
    private scoreScript score;
    private CHScript chScript;
    public float acceleration = 0.7f;
    private float currentSpeed = 0f;
    private float maxSpeed;
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Store the original color
    private bool isFlashing = false; // To prevent multiple flashes at once
    private bool isDead = false; // To prevent multiple calls to Die()

    public AudioClip cyDieSound;  
    public AudioSource audioSource; 

    
    private CinemachineImpulseSource impulseSource;

    // Colors for the dynamic flashing effect
    private Color[] flashColors = new Color[3];

    // Reference to the death particles
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
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // Initialize particle system
        deathParticles = GetComponent<ParticleSystem>();
        if (deathParticles != null)
        {
            deathParticles.Stop();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
        }

        // Define the flash colors with translucency
        flashColors[0] = new Color(1f, 1f, 1f, 0.99f);  // White with slight transparency
        flashColors[1] = new Color(0.95f, 0.95f, 0.95f, 0.98f);  // Slightly off-white
        flashColors[2] = new Color(0.90f, 0.90f, 0.90f, 0.97f);  // More subdued off-white

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

        // Start the flashing coroutine
        StartCoroutine(FlashColors());
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

        // Flash red when hit, but only if not already flashing red
        if (!isFlashing && spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (chScript != null)
        {
            chScript.MirrorHealth((int)health);
        }

        if (health <= 0 && !isDead)
        {
            Die();
        }

        AliveShake();
    
        
    }

    // Coroutine to flash red for 0.25 seconds
    IEnumerator FlashRed()
    {
        isFlashing = true;
        spriteRenderer.color = Color.red; // Change to red
        yield return new WaitForSeconds(0.125f); 
        spriteRenderer.color = originalColor; // Reset to original color
        isFlashing = false;
    }

    // Coroutine to flash between the three colors to give a sense of life
    IEnumerator FlashColors()
    {
        int index = 0;

        while (!isDead)
        {
            if (!isFlashing) // Only flash colors if not flashing red
            {
                spriteRenderer.color = flashColors[index];
                index = (index + 1) % flashColors.Length;
            }

            yield return new WaitForSeconds(0.1f); // Quickly flash colors (can be adjusted for timing)
        }
    }

    void Die()
    {
        PlayDestoryedSound();
        if (isDead) return; // Ensure Die() is only called once

        isDead = true;

        // Ensure health is 0 and stop flashing
        health = 0;
        if (chScript != null)
        {
            chScript.MirrorHealth((int)health);
        }
        StopAllCoroutines();

        DisableColliders();

        spriteRenderer.color = Color.clear; // Make the cyclops invisible
        speed = 0; // Stop moving

        // Play the death particles
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        // Handle other death logic
        killCounter.IncreaseKillCount();
        enemiesOnScreen.Subtract();
        score.IncreaseScore(points);

        DeadShake();

       

        // Destroy the cyclops after the particle effect finishes
        // Destroy(gameObject, deathParticles.main.duration);
        Destroy(gameObject, 1.5f);
    }

    void PlayDestoryedSound()
    {
        if (audioSource != null && cyDieSound != null)
        {
            // Ensure that the sound only plays on death and not on spawn
            audioSource.clip = cyDieSound;
            audioSource.Play();
        }
    }

void AliveShake()
{
    // Custom velocity for AliveShake impulse
    Vector3 aliveImpulseVelocity = new Vector3(-0.15f, -0.15f, 0f);
    GenerateImpulseWithCustomVelocity(aliveImpulseVelocity);
}

void DeadShake()
{
    // Custom velocity for DeadShake impulse
    Vector3 deadImpulseVelocity = new Vector3(-0.7f, -0.7f, 0f);
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
    void DisableColliders()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
}






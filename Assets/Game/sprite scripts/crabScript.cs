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

    public AudioClip crabChargeSound;  // Sound for charging  
    public AudioSource audioSource;    // Audio source for explosion sound
    public AudioSource audioSource2;   // Audio source for charging sound

    // Reference to the death particles
    private ParticleSystem deathParticles;
    private Color[] flashColors = new Color[3];

    private CinemachineImpulseSource impulseSource;

    public float explosionRadius = 12f; // Radius of the explosion
    public float explosionForce = 55f; // Force of the explosion

    public float damageRadius = 5f; // How far the damage can reach
    public float explosionDamage = 20f; // How much damage the explosion causes

    public float detectionRadius = 15f; // How close the crab needs to be to play the sound

    public float deadShakeIntensity = -0.5f;
    public float aliveShakeIntensity = -0.1f;


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
        originalColor = spriteRenderer.color;

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
        // Ensure spriteRenderer is assigned before checking if the crab is on screen
        if (spriteRenderer == null)
        {
            
            return;  // Safely exit if spriteRenderer is not initialized
        }

        // Check if the crab is on screen before allowing it to charge
        if (IsCrabOnScreen())
        {
            isCharging = true;
            chargeTimer = chargeTime;
            attack_damage *= 2;
            speed = chargeSpeed;

            // Play charge sound only if the crab is on screen and near the camera
            if (IsCrabNearMainCamera())
            {
                PlayChargeSound();
            }
        }
        
    }

    void EndCharging()
    {
        isCharging = false;
        speed = originalSpeed;
        attack_damage /= 2;

        if (audioSource2.isPlaying)
        {
            audioSource2.Stop();
        }   
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

        if (audioSource2.isPlaying)
        {
            audioSource2.Stop();
        }   

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

        ExplodeEnemiesAway(transform.position, explosionRadius, explosionForce, explosionDamage, damageRadius);

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
            audioSource.clip = crabDieSound;
            audioSource.Play();
        }
    }

    void PlayChargeSound()
    {
        if (audioSource2 != null && crabChargeSound != null)
        {
            audioSource2.clip = crabChargeSound;
            audioSource2.Play();
        }
    }

    // Detects if the crab is near the camera within a certain distance
    bool IsCrabNearMainCamera()
    {
        Vector3 mainCameraPosition = Camera.main.transform.position;
        float distanceToCamera = Vector3.Distance(transform.position, mainCameraPosition);
        return distanceToCamera <= detectionRadius;
    }

    // Detects if the crab is on the screen (in the camera's view)
    bool IsCrabOnScreen()
    {
        if (spriteRenderer == null || Camera.main == null)
        {
            Debug.LogWarning("SpriteRenderer or Camera is null.");
            return false;  // Safely return false if spriteRenderer or Camera.main is null
        }

        // Get the camera's frustum planes
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Get the bounds of the crab's sprite renderer
        Bounds crabBounds = spriteRenderer.bounds;

        // Check if the crab's bounds intersect with the camera's frustum planes
        return GeometryUtility.TestPlanesAABB(planes, crabBounds);
    }

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
}









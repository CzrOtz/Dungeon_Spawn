using UnityEngine;
using System.Collections;
using Cinemachine;
using System.IO;
using Unity.VisualScripting;

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

    [Header("Cyclops path")]
    private GameObject cyclopsPath1;
    private GameObject cyclopsPath2;

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
        cyclopsPath1 = GameObject.FindWithTag("Path1");
        cyclopsPath2 = GameObject.FindWithTag("Path2");

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

        // Check if the Cyclops should pursue a path
        MoveTowardsPath();
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

    void MoveTowardsPath()
    {
        if (cyclopsPath1 == null || cyclopsPath2 == null)
        {
            Debug.LogWarning("Paths are not set, moving towards the hero.");
            MoveTowardsHero();
            return;
        }

        // Choose a random path (cyclopsPath1 or cyclopsPath2)
        GameObject selectedPath = Random.Range(1, 4) > 3 ? cyclopsPath1 : cyclopsPath2;

        // Calculate distances to the path and hero
        float distanceToPath = Vector3.Distance(transform.position, selectedPath.transform.position);
        float distanceToHero = Vector3.Distance(transform.position, heroTransform.position);

        Debug.Log("FROM MOVE TOWARDS PATH DISTANCE TO HERO = " + distanceToHero);
        Debug.Log("DISTANCE TO PATH = " + distanceToPath);

        // New condition to only pursue if far enough from hero
        if (distanceToPath <= 5f && distanceToHero > 5f)
        {
            // Pursue the path for 3 seconds
            StartCoroutine(PursuePathForSeconds(selectedPath, 3f));
        }
        else
        {
            Debug.Log("Skipping pathfinding, moving towards the hero.");
            MoveTowardsHero();
        }
    }

    IEnumerator PursuePathForSeconds(GameObject targetPath, float duration)
    {
        float elapsedTime = 0f;

        // Attempt to move towards the path for the full duration or until proximity conditions are met
        while (elapsedTime < duration)
        {
            float distanceToPath = Vector3.Distance(transform.position, targetPath.transform.position);
            float distanceToHero = Vector3.Distance(transform.position, heroTransform.position);

            Debug.Log("Pursuing path... Time Elapsed: " + elapsedTime + " / " + duration);
            Debug.Log("DISTANCE TO PATH = " + distanceToPath + ", DISTANCE TO HERO = " + distanceToHero);

            // Keep moving towards the path unless abort conditions are met
            if (distanceToPath <= 2f)
            {
                Debug.Log("Reached the path, aborting pathfinding.");
                MoveTowardsHero();
                yield break;
            }

            if (distanceToHero <= 5f)
            {
                Debug.Log("Hero too close, aborting pathfinding.");
                MoveTowardsHero();
                yield break;
            }

            // Actually move towards the path
            transform.position = Vector3.MoveTowards(transform.position, targetPath.transform.position, currentSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            // Wait for next frame
            yield return null;
        }

        Debug.Log("Pursuit of the path is over, switching to hero.");
        MoveTowardsHero();
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
        LayerMask explosionLayerMask = LayerMask.GetMask("Enemy", "Hero");
        Collider2D[] enemiesForForce = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, explosionLayerMask);

        foreach (Collider2D enemy in enemiesForForce)
        {
            // Ensure the enemy has a Rigidbody2D and isn't the current exploding enemy
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null && enemy.gameObject != this.gameObject)
            {
                Debug.Log("Applying explosion force to: " + enemy.gameObject.name + " on layer: " + LayerMask.LayerToName(enemy.gameObject.layer));
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










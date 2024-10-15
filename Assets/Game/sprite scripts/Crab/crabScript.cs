using UnityEngine;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.AI;

public class crabScript : MonoBehaviour
{
    public float speed;    public float health;
    public float attack_damage;
    public Transform heroTransform;
    public float chargeTime = 2f;
    private int points = 27;

    private float chargeTimer = 0f;
    private KcountScript killCounter;
    private eosScript enemiesOnScreen;
    private scoreScript score;
    private CrabHTScript crabHScript;
    private float originalSpeed;
    private float chargeSpeed;
    private bool isCharging = false;
    private bool isDead = false; // To prevent multiple death calls

    [Header("Audio")]
    public AudioClip crabDieSound;
    public AudioClip crabChargeSound;  // Sound for charging  
    public AudioSource audioSource;    // Audio source for explosion sound
    public AudioSource audioSource2;   // Audio source for charging sound

    [Header("Detection Radius")]
    public float detectionRadius = 15f; // How close the crab needs to be to play the sound

    // Reference to the death particles
    private ParticleSystem deathParticles;

    private CinemachineImpulseSource impulseSource;

    [Header("Explosion Visual Physics")]
    public float explosionRadius = 12f; // Radius of the explosion
    public float explosionForce = 55f; // Force of the explosion

    [Header("Explosion Damage")]
    public float damageRadius = 5f; // How far the damage can reach
    public float explosionDamage = 20f; // How much damage the explosion causes


    [Header("Shake Intensity")]
    public float deadShakeIntensity = -0.5f;
    public float aliveShakeIntensity = -0.1f;

    // Reference to crabRender script
    private crabRender renderScript;

    //this is for AI

    private NavMeshAgent agent;

    public void Initialize(float initialSpeed, float initialDamage, float initialHealth)
    {
        speed = initialSpeed;
        attack_damage = initialDamage;
        health = initialHealth;
        originalSpeed = speed;
        chargeSpeed = Random.Range(speed * 3f, speed * 7f);
    }

    void Start()
    {
        killCounter = FindObjectOfType<KcountScript>();
        score = FindObjectOfType<scoreScript>();
        enemiesOnScreen = FindAnyObjectByType<eosScript>();
        crabHScript = GetComponentInChildren<CrabHTScript>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        // Get the render script from the child
        renderScript = GetComponentInChildren<crabRender>();
        if (renderScript == null)
        {
            Debug.LogError("crabRender script not found in child object!");
        }

        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            Debug.LogError("CinemachineImpulseSource not found on crab!");
        }

        // Initialize particles and ensure they don't play at the start
        deathParticles = GetComponent<ParticleSystem>();
        if (deathParticles != null)
        {
            deathParticles.Stop();
        }
        else
        {
            Debug.LogError("ParticleSystem (deathParticles) not found on crab!");
        }

        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        if (hero != null)
        {
            heroTransform = hero.transform;
        }
        else
        {
            Debug.LogError("Hero not found! Ensure the hero has the correct tag.");
        }
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

            //always keep the speed updated due to changing behaviors
            agent.speed = speed;
        }

        
    }

    void MoveTowardsHero()
    {
        float currentSpeed = isCharging ? chargeSpeed : speed;
        // transform.position = Vector3.MoveTowards(transform.position, heroTransform.position, currentSpeed * Time.deltaTime);
        agent.SetDestination(heroTransform.position);
        
    }



    public void StartCharging()
    {
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

            // Notify render script to start charging visuals
            if (renderScript != null)
            {
                renderScript.StartChargingVisuals();
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

        // Notify render script to end charging visuals
        if (renderScript != null)
        {
            renderScript.EndChargingVisuals();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (renderScript != null)
        {
            renderScript.StartCoroutine(renderScript.FlashRed());
        }

        if (health <= 0 && !isDead)
        {
            Die();
            health = 0;
        }

        AliveShake();
    }

    void Die()
    {
        PlayDestroyedSound();

        

        if (audioSource2.isPlaying)
        {
            audioSource2.Stop();
        }

        if (isDead) return;

        isDead = true;
        health = 0;

        if (crabHScript != null)
        {
            crabHScript.MirrorHealth((int)health);
        }

        // Disable colliders to prevent further interactions
        DisableColliders();

        speed = 0;

        // Play the death particles
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        ExplodeEnemiesAway(transform.position, explosionRadius, explosionForce, explosionDamage, damageRadius);

        // Notify render script to handle death visuals
      
        renderScript.Die();
        

        // Handle other death logic
        killCounter.IncreaseKillCount();
        enemiesOnScreen.Subtract();
        score.IncreaseScore(points);

        DeadShake();

        // Destroy the crab after the particle effect finishes
        Destroy(gameObject, deathParticles.main.duration);
    }

    void PlayDestroyedSound()
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
        if (renderScript == null || Camera.main == null)
        {
            return false;
        }

        // Get the camera's frustum planes
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Get the bounds of the crab's sprite renderer from render script
        Bounds crabBounds = renderScript.GetSpriteBounds();

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









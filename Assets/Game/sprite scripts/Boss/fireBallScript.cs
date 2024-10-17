using UnityEngine;
using Cinemachine;

public class fireBallScript : MonoBehaviour
{
    [SerializeField] Transform hero;  // Reference to the hero
    public float moveSpeed = 5f;      // Movement speed towards the hero
    public float waitForLaunch = 5f;  // Time before the fireball starts moving (set individually)
    public float lifeTime = 10f;      // Time the fireball will live after being launched

    public CinemachineImpulseSource impulseSource;

    [Header("Explosion Settings")]
    public float explosionRadius = 5f;                   // Radius of the explosion
    public float explosionForce = 200f;                  // Maximum force of the explosion
    public float explosionDamage = 100f;                 // Maximum damage caused by explosion
    public float heroExplosionDamageModifier = 2f;       // Damage modifier for the hero

    [Header("Explosion Falloff Curve")]
    public AnimationCurve explosionFalloff = AnimationCurve.Linear(0, 1, 1, 0); // Force falloff over distance

    [Header("Explosion Effects")]
    public fireBallVisualScript fbVisuals;  // Reference to explosion visuals script

    private float timer = 0f;
    private bool isLaunched = false;
    private bool isRedirected = false;
    private bool isDead = false;

    private Collider2D fbnCollider;
    private Rigidbody2D fbRigidbody;
    private fireballRender renderScript;

    // Orbit variables
    private Transform boss;
    private float orbitRadius;
    private float orbitSpeed;    // In radians per second
    private float currentAngle;

    void Start()
    {
        fbnCollider = GetComponent<Collider2D>();
        fbRigidbody = GetComponent<Rigidbody2D>();

        // Ensure the hero reference is set
        if (hero == null)
        {
            GameObject heroObj = GameObject.FindGameObjectWithTag("Hero");
            if (heroObj != null)
            {
                hero = heroObj.transform;
            }
            else
            {
                Debug.LogError("Hero not found in scene!");
            }
        }

        renderScript = GetComponentInChildren<fireballRender>();
        if (renderScript != null)
        {
            renderScript.StartFireEffect();
        }

        TurnOffPhysicsUntilLaunch();
    }

    void Update()
    {
        timer += Time.deltaTime;

        TurnOffPhysicsUntilLaunch();

        if (!isLaunched)
        {
            OrbitAroundBoss();
        }

        // Launch the fireball after waiting
        if (timer >= waitForLaunch && !isLaunched)
        {
            isLaunched = true;
        }

        // Move towards hero after launch
        if (isLaunched && !isDead && !isRedirected)
        {
            MoveTowardsHero();
        }

        // Destroy the fireball after its lifetime expires
        if (timer >= waitForLaunch + lifeTime)
        {
            Die();
        }
    }

    // Orbit around the boss while waiting for launch
    void OrbitAroundBoss()
    {
        if (boss != null)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
            Vector3 offset = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * orbitRadius;
            transform.position = boss.position + offset;
        }
    }

    // Set parameters for orbiting around the boss
    public void SetOrbitParams(Transform bossTransform, float radius, float speedDegreesPerSecond, float startAngleRadians)
    {
        boss = bossTransform;
        orbitRadius = radius;
        orbitSpeed = speedDegreesPerSecond * Mathf.Deg2Rad; // Convert degrees to radians
        currentAngle = startAngleRadians;
    }

    // Move towards the hero after being launched
    public void MoveTowardsHero()
    {
        if (hero != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, hero.position, moveSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
    }

    // Handle collision with spear and hero
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Spear"))
        {
            // Redirect the fireball by stopping the movement towards the hero
            isRedirected = true;

            // Generate camera shake
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }

            // Ensure Rigidbody2D is non-kinematic and apply force from the spear
            fbRigidbody.isKinematic = false;
            Vector2 forceDirection = collision.relativeVelocity.normalized;
            float forceMagnitude = collision.relativeVelocity.magnitude * 1f; // Adjust multiplier as needed
            fbRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        }
        else if (collision.gameObject.CompareTag("Hero"))
        {
            // Explode upon collision with hero
            Die();
        }
    }

    // Emit explosion and apply damage
    void EmitExplosion()
    {
        // Generate camera shake
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Play explosion particles and sound
        if (fbVisuals != null)
        {
            fbVisuals.Particles();
            fbVisuals.Sound();
        }
        else
        {
            Debug.LogError("Explosion visuals not assigned in fireBallScript.");
        }

        // Apply explosion force and damage
        Explode();
    }

    // Apply explosion forces and damage to objects in range
    void Explode()
    {
        Vector2 explosionPosition = transform.position;
        LayerMask layerMask = LayerMask.GetMask("Default", "Hero", "Boss"); // Include "Boss" layer if necessary

        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, layerMask);

        foreach (Collider2D obj in objectsInRange)
        {
            Vector2 objPosition = obj.transform.position;
            Vector2 dir = objPosition - explosionPosition;
            float distance = dir.magnitude;

            if (distance == 0)
            {
                distance = 0.1f;
            }

            float normalizedDistance = distance / explosionRadius;
            normalizedDistance = Mathf.Clamp01(normalizedDistance);

            float forceMultiplier = explosionFalloff.Evaluate(normalizedDistance);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceMagnitude = explosionForce * forceMultiplier;
                rb.AddForce(dir.normalized * forceMagnitude, ForceMode2D.Impulse);
            }

            float damage = explosionDamage * forceMultiplier;

            if (obj.CompareTag("Hero"))
            {
                damage /= heroExplosionDamageModifier;
                obj.GetComponent<heroScript>().health -= damage;
            }

            if (obj.CompareTag("Boss"))
            {
                var bossScript = obj.GetComponent<testAgentScript>();
                if (bossScript != null)
                {
                    bossScript.TakeDamage(damage);
                }
            }
        }
    }

    // Destroy the fireball upon death
    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // Disable physics and movement
        fbRigidbody.isKinematic = true;
        fbRigidbody.velocity = Vector2.zero;
        fbnCollider.enabled = false;

        if (renderScript != null)
        {
            renderScript.StopFireEffect();
            renderScript.HideSprites();
        }

        EmitExplosion();

        Destroy(gameObject, 1f);  // Adjust delay if necessary
    }

    void TurnOffPhysicsUntilLaunch()
    {
        if (!isLaunched)
        {
            fbnCollider.enabled = false;
            fbRigidbody.isKinematic = true;
        }
        else
        {
            fbnCollider.enabled = true;
            fbRigidbody.isKinematic = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}




using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Cinemachine;

public class testAgentScript : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] List<ParticleSystem> particleSystems;  // List to hold particle systems
    public float health = 20000f; // Boss health
    private spearSpawnerScript spearSpawner;
    public BossHealthText healthText;

    public CinemachineImpulseSource impulseSource; // Cinemachine impulse source

    [Header("Shake Intensities")]
    public float normalShakeIntensity = -0.1f;   // Normal shake intensity when hit
    public float explosionShakeIntensity = -0.5f; // Stronger shake for particle emission hit

    [Header("Explosion Visual Physics")]
    public float explosionRadius = 50f;  // Radius of the explosion
    public float explosionForce = 200f;  // Maximum force of the explosion
    public float explosionDamage = 100f; // Maximum damage caused by explosion
    public float heroExplosionDamageModifier = 2f; // Reduce damage for the hero

    [Header("Explosion Falloff Curve")]
    public AnimationCurve explosionFalloff = AnimationCurve.Linear(0, 1, 1, 0); // Force falloff over distance

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spearSpawner = FindObjectOfType<spearSpawnerScript>();

        if (healthText == null)
        {
            Debug.LogError("healthText not assigned in the inspector!");
        }

        if (impulseSource == null)
        {
            Debug.LogError("CinemachineImpulseSource not assigned!");
        }
    }

    void Update()
    {
        agent.SetDestination(target.position);
        healthText.MirrorHealth((int)health);  // Update the health text
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spear"))
        {
            // Decrease health
            health -= spearSpawner.damage;

            // Determine if particles should be emitted
            bool shouldEmit = EmitParticles();

            // Trigger appropriate shake and explosion
            if (shouldEmit)
            {
                TriggerShake(explosionShakeIntensity); // Stronger shake with particles
            }
            else
            {
                TriggerShake(normalShakeIntensity);  // Regular shake without particles
            }
        }
    }

    void TriggerShake(float shakeIntensity)
    {
        impulseSource.m_DefaultVelocity = new Vector3(shakeIntensity, shakeIntensity, 0);
        impulseSource.GenerateImpulse(); // Trigger shake with given intensity
    }

    bool EmitParticles()
    {
        int lotteryDraw = Random.Range(1, 11);  // Random draw from 1 to 10
        List<int> validNumbers = new List<int> { 2, 4, 5, 7 };  // Valid numbers for emission

        if (validNumbers.Contains(lotteryDraw))
        {
            int index = validNumbers.IndexOf(lotteryDraw);
            if (index < particleSystems.Count)
            {
                particleSystems[index].Play();  // Play the corresponding particle system

                ExplodeHeroAway(); // Push back the hero when particles are emitted

                return true; // Particle emitted
            }
        }
        return false; // No particles emitted
    }

    void ExplodeHeroAway()
    {
        Vector2 explosionPosition = transform.position;
        LayerMask layerMask = LayerMask.GetMask("Hero"); // Only affect the hero
        Collider2D[] heroesInRange = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, layerMask);

        foreach (Collider2D hero in heroesInRange)
        {
            Rigidbody2D heroRb = hero.GetComponent<Rigidbody2D>();
            if (heroRb != null)
            {
                Vector2 dir = heroRb.position - explosionPosition;
                float distance = dir.magnitude;

                // Avoid division by zero
                if (distance == 0)
                {
                    distance = 0.1f;
                }

                // Calculate normalized distance (0 at explosion center, 1 at explosionRadius)
                float normalizedDistance = distance / explosionRadius;
                normalizedDistance = Mathf.Clamp01(normalizedDistance);

                // Evaluate the force multiplier from the curve
                float forceMultiplier = explosionFalloff.Evaluate(normalizedDistance);

                // Calculate force magnitude
                float forceMagnitude = explosionForce * forceMultiplier;

                // Apply the force
                heroRb.AddForce(dir.normalized * forceMagnitude, ForceMode2D.Impulse);

                // Apply explosion damage to the hero
                float damage = (explosionDamage * forceMultiplier) / heroExplosionDamageModifier;
                hero.GetComponent<heroScript>().health -= damage;

                Debug.Log("Applying explosion force and damage to hero: " + damage + " with force magnitude: " + forceMagnitude);
            }
        }
    }

    // Optional: Visualize the explosion radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}




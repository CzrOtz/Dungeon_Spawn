using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class spearScript : MonoBehaviour
{
    public bool isMoving = false;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Camera cam;

    public AudioClip spearThrowSound;  // Sound for throwing the spear
    public AudioSource audioSource;    // AudioSource component assigned via the Inspector

    private spearSpawnerScript spearSpawner;
    private spearCollisionScript collisionManagerS;

    private float maxFlightTime = 1.5f; // Maximum flight time for the spear
    private Collider2D spearCollider;   // Reference to the spear's collider

    private heroScript hero; // Reference to the hero

    void Start()
    {
        spearSpawner = FindObjectOfType<spearSpawnerScript>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        hero = FindObjectOfType<heroScript>();

        // Get the spear's collider
        spearCollider = GetComponent<Collider2D>();

        collisionManagerS = FindAnyObjectByType<spearCollisionScript>();

        // Disable collisions at the start since the spear is not moving
        DisableCollisions();
    }

    void Update()
    {
        if (!isMoving)
        {
            AimAtCursor();

            if (Input.GetMouseButtonDown(0))
            {
                StartMovement();
            }
        }

        if (hero.isDead)
        {
            isMoving = false;
        }
    }

    void AimAtCursor()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void StartMovement()
    {
        isMoving = true;

        // Enable collisions when spear starts moving
        EnableCollisions();

        // Play spear throw sound
        if (audioSource != null && spearThrowSound != null && !hero.isDead)
        {
            AudioSource.PlayClipAtPoint(spearThrowSound, transform.position, audioSource.volume);
        }

        // Detach the spear from the spawner
        transform.SetParent(null);  // Spear is fully detached from the spawner

        // Calculate the movement direction
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        moveDirection = (mousePos - transform.position).normalized;

        // Reset velocity to ensure no residual velocity from physics
        rb.velocity = Vector2.zero;

        // Apply the calculated velocity for the spear movement
        rb.velocity = moveDirection * spearSpawner.speed;

        // Start the countdown to destroy the spear after maxFlightTime seconds
        StartCoroutine(DestroyAfterTime(maxFlightTime));
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Keep the spear moving at the set speed in the given direction
            rb.velocity = moveDirection * spearSpawner.speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only manage collisions if spear is moving
        if (isMoving)
        {
            collisionManagerS.ManageCollision(collision);
        }
    }

    // Disable collisions when the spear is stationary
    void DisableCollisions()
    {
        if (spearCollider != null)
        {
            spearCollider.enabled = false;
        }
    }

    // Enable collisions when the spear is moving
    void EnableCollisions()
    {
        if (spearCollider != null)
        {
            spearCollider.enabled = true;
        }
    }

    // Coroutine to destroy the spear after a certain time
    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (isMoving)  // Check if the spear is still flying
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void DestroySpear()
    {
        Destroy(gameObject);
    }
}







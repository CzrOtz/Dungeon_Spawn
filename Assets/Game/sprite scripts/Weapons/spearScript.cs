using System.Collections;
using UnityEngine;

public class spearScript : MonoBehaviour
{
    public bool isMoving = false;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Camera cam;

    public AudioClip spearThrowSound;  // Sound for throwing the spear
    public AudioSource audioSource;    // AudioSource component assigned via the Inspector

    private spearCollisionScript collisionManagerS;

    public float speed = 20f;
    public float damage = 15f;

    private float maxFlightTime = 1f; // Maximum flight time for the spear
    private Collider2D spearCollider;   // Reference to the spear's collider

    private heroScript hero; // Reference to the hero

    // Variables for color flashing
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public bool isFlashing = false;
    private Coroutine flashCoroutine;

    // For color cycling
    private Color[] flashColors;
    private float flashDuration;
    private float flashInterval;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        hero = FindObjectOfType<heroScript>();

        // Get the spear's collider
        spearCollider = GetComponent<Collider2D>();

        collisionManagerS = FindAnyObjectByType<spearCollisionScript>();

        // Disable collisions at the start since the spear is not moving
        DisableCollisions();

        // Get the SpriteRenderer component and store the original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

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
        rb.velocity = moveDirection * speed;

        // Start the countdown to destroy the spear after maxFlightTime seconds
        StartCoroutine(DestroyAfterTime(maxFlightTime));
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Keep the spear moving at the set speed in the given direction
            rb.velocity = moveDirection * speed;
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

    // Method to set the spear's color
    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    // Method to start flashing
    public void StartFlashing(Color[] colors, float duration, float interval)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashColors = colors;
        flashDuration = duration;
        flashInterval = interval;

        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        float elapsedTime = 0f;
        int colorIndex = 0;
        int colorsCount = flashColors.Length;

        while (elapsedTime < flashDuration)
        {
            SetColor(flashColors[colorIndex]);
            colorIndex = (colorIndex + 1) % colorsCount;

            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        // Reset to original color
        SetColor(originalColor);
        isFlashing = false;
        flashCoroutine = null;
    }
}






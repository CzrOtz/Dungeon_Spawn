using UnityEngine;

public class heroRenderScript : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.05f; // Height of the bounce
    public float bounceFrequency = 2f;    // Speed of the bounce

    private float bounceTime;
    private Vector3 originalLocalPosition;

    private Rigidbody2D heroRigidbody;
    private heroScript hero;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;

    private bool isFlashing = false;      // Indicates if a power-up flash is active
    private Color currentFlashColor;      // The current color from the flash effect

    void Start()
    {
        // Store the original local position of the sprite
        originalLocalPosition = transform.localPosition;

        // Get the Rigidbody2D from the parent (hero)
        heroRigidbody = GetComponentInParent<Rigidbody2D>();
        if (heroRigidbody == null)
        {
            Debug.LogError("Rigidbody2D not found in parent!");
        }

        // Get the heroScript from the parent
        hero = GetComponentInParent<heroScript>();
        if (hero == null)
        {
            Debug.LogError("heroScript not found in parent!");
        }

        // Get the SpriteRenderer from this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject!");
        }
        else
        {
            originalColor = spriteRenderer.color; // Save the original color to reset later
        }
    }

    void Update()
    {
        // Handle bouncing
        HandleBounce();

        // Handle sprite color based on hero's state
        HandleSpriteColor();
    }

    void HandleBounce()
    {
        if (hero != null && hero.isDead)
        {
            // Hero is dead, reset bounce and position
            bounceTime = 0f;
            transform.localPosition = originalLocalPosition;
            return;
        }

        if (heroRigidbody != null && heroRigidbody.velocity.magnitude > 0.01f)
        {
            // Hero is moving, apply bounce
            bounceTime += Time.deltaTime * bounceFrequency;
            float bounceOffset = Mathf.Sin(bounceTime) * bounceAmplitude;
            transform.localPosition = originalLocalPosition + new Vector3(0f, bounceOffset, 0f);
        }
        else
        {
            // Hero is stationary, reset bounce
            bounceTime = 0f;
            transform.localPosition = originalLocalPosition;
        }
    }

    void HandleSpriteColor()
    {
        if (spriteRenderer == null || hero == null)
            return;

        if (hero.isDead)
        {
            // Highest priority: Hero is dead, turn sprite black
            spriteRenderer.color = Color.black;
        }
        else if (hero.takingDamage)
        {
            // Next priority: Hero is taking damage, turn sprite red
            spriteRenderer.color = Color.red;
        }
        else if (isFlashing)
        {
            // Next priority: Hero is flashing due to power-up
            spriteRenderer.color = currentFlashColor;
        }
        else
        {
            // Default state: Hero is alive and not taking damage or flashing
            spriteRenderer.color = originalColor;
        }
    }

    public void FlashGreen()
    {
        StartCoroutine(FlashGreenCoroutine());
    }

    private System.Collections.IEnumerator FlashGreenCoroutine()
    {
        isFlashing = true;

        float flashDuration = 1.5f;
        float flashInterval = 0.09f;
        Color[] greenShades = new Color[]
        {
            new Color(0f, 0.85f, 0f, 0.90f),   // Bright Green
            new Color(0f, 0.75f, 0f, 0.93f), // Medium Green
            new Color(0f, 0.6f, 0f, 0.95f)  // Dark Green
        };

        float elapsedTime = 0f;
        int colorIndex = 0;

        while (elapsedTime < flashDuration)
        {
            currentFlashColor = greenShades[colorIndex];
            colorIndex = (colorIndex + 1) % greenShades.Length; // Cycle through the shades of green

            elapsedTime += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        isFlashing = false;
    }

    public void FlashBlue()
    {
        StartCoroutine(FlashBlueCoroutine());
    }

    private System.Collections.IEnumerator FlashBlueCoroutine()
    {
        isFlashing = true;

        float flashDuration = 1.5f;
        float flashInterval = 0.09f;
        Color[] blueShades = new Color[]
        {
            new Color(0f, 0f, 0.85f, 0.90f),   // Bright Blue
            new Color(0f, 0f, 0.75f, 0.93f), // Medium Blue
            new Color(0f, 0f, 0.6f, 0.95f)  // Dark Blue
        };

        float elapsedTime = 0f;
        int colorIndex = 0;

        while (elapsedTime < flashDuration)
        {
            currentFlashColor = blueShades[colorIndex];
            colorIndex = (colorIndex + 1) % blueShades.Length; // Cycle through the shades of blue

            elapsedTime += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        isFlashing = false;
    }
}




using UnityEngine;
using System.Collections;

public class cyclopsRenderScript : MonoBehaviour
{
    // Bounce effect variables
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.05f; // Height of the bounce
    public float bounceFrequency = 2f;    // Speed of the bounce
    private float bounceTime;
    private Vector3 originalLocalPosition;

    // Visual components
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false;

    // Flashing colors
    private Color[] flashColors = new Color[3];

    // Death particles
   

    // Reference to cyclopsScript
    private cyclopsScript cyclops;

    void Start()
    {
        // Store original local position for bouncing
        originalLocalPosition = transform.localPosition;

        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on cyclopsRenderScript!");
        }

        // Get death particles
        

        // Define flash colors
        flashColors[0] = new Color(1f, 1f, 1f, 0.99f);
        flashColors[1] = new Color(0.95f, 0.95f, 0.95f, 0.98f);
        flashColors[2] = new Color(0.90f, 0.90f, 0.90f, 0.97f);

        // Get cyclopsScript from parent
        cyclops = GetComponentInParent<cyclopsScript>();
        if (cyclops == null)
        {
            Debug.LogError("cyclopsScript not found on parent object!");
        }

        // Start flashing coroutine
        StartCoroutine(FlashColors());
    }

    void Update()
    {
        // Handle bounce effect
        HandleBounce();
    }

    void HandleBounce()
    {
        if (cyclops != null && !isDead)
        {
            // Apply bounce
            bounceTime += Time.deltaTime * bounceFrequency;
            float bounceOffset = Mathf.Sin(bounceTime) * bounceAmplitude;
            transform.localPosition = originalLocalPosition + new Vector3(0f, bounceOffset, 0f);
        }
        else
        {
            // Reset bounce when dead
            bounceTime = 0f;
            transform.localPosition = originalLocalPosition;
        }
    }

    // Coroutine to flash red when taking damage
    public IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            isFlashing = true;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.125f);
            spriteRenderer.color = originalColor;
            isFlashing = false;
        }
    }

    // Coroutine for dynamic flashing effect
    IEnumerator FlashColors()
    {
        int index = 0;

        while (!isDead)
        {
            if (!isFlashing && spriteRenderer != null)
            {
                spriteRenderer.color = flashColors[index];
                index = (index + 1) % flashColors.Length;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Method to handle death visuals
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop coroutines
        StopAllCoroutines();

        // Make invisible
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.clear;
        }

        
        // Destroy after delay (ensure parent is destroyed)
        Destroy(transform.parent.gameObject, 1.5f);
    }
}


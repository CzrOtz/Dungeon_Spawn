using UnityEngine;
using System.Collections;

public class crabRender : MonoBehaviour
{
    // Visual components
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false;
    private bool isChargingVisual = false;

    // Bounce effect variables
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.05f; // Height of the bounce
    public float bounceFrequency = 2f;    // Speed of the bounce
    private float bounceTime;
    private Vector3 originalLocalPosition;

    // Flashing colors
    private Color[] flashColors = new Color[3];

    // Reference to crabScript
    private crabScript crab;

    void Start()
    {
        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on crabRender!");
        }

        // Store original local position for bouncing
        originalLocalPosition = transform.localPosition;

        // Define flash colors
        flashColors[0] = new Color(1f, 1f, 1f, 0.97f);
        flashColors[1] = new Color(0.95f, 0.95f, 0.95f, 0.98f);
        flashColors[2] = new Color(0.90f, 0.90f, 0.90f, 0.99f);

        // Get crabScript from parent
        crab = GetComponentInParent<crabScript>();
        if (crab == null)
        {
            Debug.LogError("crabScript not found on parent object!");
        }

        // Start flashing coroutine
        StartCoroutine(FlashColors());
    }

    void Update()
    {
        HandleBounce();
    }

    void HandleBounce()
    {
        if (crab != null && !isDead)
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
            if (!isFlashing && !isChargingVisual)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = flashColors[index];
                    index = (index + 1) % flashColors.Length;
                }
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
        
        spriteRenderer.color = Color.clear;
      

        // Reset bounce
        bounceTime = 0f;
        transform.localPosition = originalLocalPosition;
    }

    // Method to start charging visuals
    public void StartChargingVisuals()
    {
        if (spriteRenderer != null)
        {
            isChargingVisual = true;
            StartCoroutine(ChargeFlash());
        }
    }

    // Method to end charging visuals
    public void EndChargingVisuals()
    {
        isChargingVisual = false;
        StopCoroutine("ChargeFlash");
        spriteRenderer.color = originalColor;
    }

    // Coroutine for charge flashing effect
    IEnumerator ChargeFlash()
    {
        while (isChargingVisual)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.yellow;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield break;
            }
        }
    }

    // Method to get the sprite bounds for visibility checks
    public Bounds GetSpriteBounds()
    {
        if (spriteRenderer != null)
        {
            return spriteRenderer.bounds;
        }
        else
        {
            return new Bounds();
        }
    }
}


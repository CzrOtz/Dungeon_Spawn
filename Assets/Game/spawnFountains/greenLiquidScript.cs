using UnityEngine;
using System.Collections;

public class greenLiquidScript : MonoBehaviour
{
    // Three shades of green with varying hues and opacity
    private Color green1 = new Color(0.25f, 1.0f, 0.25f, 0.8f);  // Lighter green
    private Color green2 = new Color(0.18f, 0.8f, 0.18f, 0.6f);  // Medium green
    private Color green3 = new Color(0.1f, 0.6f, 0.1f, 0.7f);    // Darker green

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    public float flashSpeed = 0.3f;  // Time in seconds between flashes

    void Start()
    {
        // Get the SpriteRenderer component from this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Start the flashing coroutine
        flashCoroutine = StartCoroutine(FlashColors());
    }

    // Coroutine that handles the color flashing
    IEnumerator FlashColors()
    {
        while (true)  // Infinite loop; stops when coroutine is killed
        {
            // Cycle through the green colors
            spriteRenderer.color = green1;
            yield return new WaitForSeconds(flashSpeed);  // Wait for the flashSpeed duration

            spriteRenderer.color = green2;
            yield return new WaitForSeconds(flashSpeed);

            spriteRenderer.color = green3;
            yield return new WaitForSeconds(flashSpeed);
        }
    }

    // Public method to stop the flashing and set the sprite to fully transparent
    public void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);  // Stop the coroutine
            flashCoroutine = null;
        }

        // Set the sprite to fully transparent
        spriteRenderer.color = new Color(0f, 0f, 0f, 0f);  // Fully transparent
    }
}


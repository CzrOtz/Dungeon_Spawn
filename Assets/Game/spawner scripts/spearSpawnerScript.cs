using System.Collections;
using UnityEngine;

public class spearSpawnerScript : MonoBehaviour
{
    public GameObject spearPrefab; // Prefab of the spear
    private GameObject currentSpear; // Reference to the currently spawned spear

    // Speed and damage are here because if not, they would be reset
    // every time a new instance of spear is instantiated
    public float speed = 20f;
    public float damage = 15f;

    // Flashing variables
    private bool isFlashing = false;
    private Color[] flashColors;
    private float flashDuration;
    private float flashInterval;
    private Coroutine flashCoroutine;

    // Original color of the spear
    private Color originalColor = Color.white; // Adjust if the default is different

    // Expose flash duration and interval as public variables
    public float defaultFlashDuration = 1.5f;  // Default flash duration in seconds
    public float defaultFlashInterval = 0.09f; // Default interval between color changes

    void Start()
    {
        // Spawn the first spear when the game starts
        SpawnNewSpear();
    }

    void Update()
    {
        // Check if the current spear is null (either fired and destroyed or never existed)
        if (currentSpear == null)
        {
            // Spawn a new spear after the previous one is destroyed
            SpawnNewSpear();
        }
        else
        {
            // Ensure the current spear stays attached to the spawner until it's fired
            spearScript spearScriptComponent = currentSpear.GetComponent<spearScript>();
            if (spearScriptComponent != null && !spearScriptComponent.isMoving)
            {
                // Keep the spear attached to the spawner's position
                currentSpear.transform.position = transform.position;

                // If the spear is not flashing but the spawner is flashing, start flashing on the spear
                if (isFlashing && !spearScriptComponent.isFlashing)
                {
                    spearScriptComponent.StartFlashing(flashColors, flashDuration, flashInterval);
                }
            }
        }
    }

    // Method to spawn a new spear
    void SpawnNewSpear()
    {
        // Instantiate a new spear at the spawner's position and rotation
        currentSpear = Instantiate(spearPrefab, transform.position, transform.rotation);

        // Set the spear's speed and damage from the spawner
        spearScript spearScriptComponent = currentSpear.GetComponent<spearScript>();
        if (spearScriptComponent != null)
        {
            spearScriptComponent.speed = speed;
            spearScriptComponent.damage = damage;

            // Pass the flashing state to the spear
            if (isFlashing)
            {
                spearScriptComponent.StartFlashing(flashColors, flashDuration, flashInterval);
            }
        }
    }

    public void FlashRed()
    {
        FlashRed(defaultFlashDuration, defaultFlashInterval);
    }

    public void FlashRed(float duration, float interval)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        // Set the flash duration and interval
        flashDuration = duration;
        flashInterval = interval;

        // Define the shades of red
        flashColors = new Color[]
        {
            new Color(0.98f, 0f, 0f, 0.85f),  // Red at 98% intensity
            new Color(0.85f, 0f, 0f, 0.95f),   // Red at 90% intensity
            new Color(0.6f, 0f, 0f)   // Red at 85% intensity
        };   

        isFlashing = true;

        // Start the coroutine to reset flashing after duration
        flashCoroutine = StartCoroutine(StopFlashingAfterDuration(duration));
    }

    public void FlashWhite()
    {
        FlashWhite(defaultFlashDuration, defaultFlashInterval);
    }

    public void FlashWhite(float duration, float interval)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        // Set the flash duration and interval
        flashDuration = duration;
        flashInterval = interval;

        // Define the shades of white (gray shades)
        flashColors = new Color[]
        {
            new Color(0.5f, 0.5f, 0.5f),        // White
            new Color(0.3f, 0.3f, 0.3f),  // Light Gray
            new Color(0.1f, 0.1f, 0.1f)   // Dark Gray
        };

        isFlashing = true;

        // Start the coroutine to reset flashing after duration
        flashCoroutine = StartCoroutine(StopFlashingAfterDuration(duration));
    }

    private IEnumerator StopFlashingAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFlashing = false;
        flashCoroutine = null;
    }
}





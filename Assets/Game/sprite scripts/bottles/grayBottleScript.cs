using UnityEngine;

public class grayBottleScript : MonoBehaviour
{
    // This script will increase the speed of the spear
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.1f;  // Height of the bounce
    public float bounceFrequency = 20f;   // Speed of the bounce
    private float bounceTime;

    private heroScript hero;
    private spearSpawnerScript spearSpawner;
    public float spearSpeedBottleIncrease = 1.18f;

    void Start()
    {
        spearSpawner = FindObjectOfType<spearSpawnerScript>();
        hero = FindAnyObjectByType<heroScript>();   
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        HandleBounce();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            if (spearSpawner.speed > 40)
            {
                spearSpawner.speed += 5;
            }
            else
            {
                spearSpawner.speed *= spearSpeedBottleIncrease;
            }

            Destroy(gameObject);
        }
    }

    void HandleBounce()
    {
        // Apply bounce effect using sine wave
        bounceTime += Time.deltaTime * bounceFrequency;
        float bounceOffset = Mathf.Sin(bounceTime) * bounceAmplitude;
    
        // Only modify the Y position (bounce) and keep X and Z as they are
        Vector3 currentPosition = transform.localPosition;
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + bounceOffset, currentPosition.z);
    }
}


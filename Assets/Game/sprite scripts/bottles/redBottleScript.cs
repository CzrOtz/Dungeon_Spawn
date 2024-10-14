using UnityEngine;

public class redBottleScript : MonoBehaviour
{
    // This script increases spear power
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.1f;  // Height of the bounce
    public float bounceFrequency = 20f;   // Speed of the bounce
    private float bounceTime;

    private heroScript hero;
    private spearSpawnerScript spearSpawner;
    private float spearDamageBottleIncrease = 2f;

    
    void Start()
    {
        // Reference hero and spear spawner in Start
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
            spearSpawner.FlashRed();

            if (spearSpawner.damage > 100)
            {
                spearSpawner.damage += 5;
            }
            else
            {
                spearSpawner.damage += spearDamageBottleIncrease;
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


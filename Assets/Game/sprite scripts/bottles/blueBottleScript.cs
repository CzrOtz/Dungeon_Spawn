using UnityEngine;

public class blueBottleScript : MonoBehaviour
{
    // This script will increase the speed of the hero
    // Starting speed should be 10 
    // Deceleration rate should be 10 constant 

    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.1f; // Height of the bounce
    public float bounceFrequency = 20f;  // Speed of the bounce
    private float bounceTime;
    
    private heroScript hero;
    private heroRenderScript heroRender;
    private float increaseSpeedRate = 1.07f;

    // Start is called before the first frame update
    void Start()
    {
        hero = FindObjectOfType<heroScript>();
        heroRender = FindObjectOfType<heroRenderScript>();
        // Debug.Log("hero.speed += increaseSpeedRate: " + (hero.speed *= increaseSpeedRate));
    }

    // Update is called once per frame
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
        heroRender.FlashBlue();

        

        if (collision.gameObject.CompareTag("Hero"))
        {
            if (hero.speed > 125)
            {
                hero.speed += 0.2f;  // Slight increment if hero speed is already high
            }
            else
            {
                hero.speed *= increaseSpeedRate;  // Otherwise, apply the speed multiplier
            }

            Destroy(gameObject);  // Destroy the bottle after it has been collected
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

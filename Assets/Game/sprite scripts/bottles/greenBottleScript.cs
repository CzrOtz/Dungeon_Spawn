using UnityEngine;

public class greenBottleScript : MonoBehaviour
{
    //this script makes contact with the heros colider
    //then it will send a signal to the heros render.
    
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.1f;  // Height of the bounce
    public float bounceFrequency = 20f;   // Speed of the bounce
    private float bounceTime;

    private heroScript hero;

    private heroRenderScript heroRender;
    public float greenBottleHealthIncrease = 25f;


    void Start()
    {
        hero = FindObjectOfType<heroScript>();
        heroRender = FindObjectOfType<heroRenderScript>();
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
            hero.health += greenBottleHealthIncrease;

            heroRender.FlashGreen();
            

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



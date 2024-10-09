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
            // Hero is dead, turn sprite black
            spriteRenderer.color = Color.black;
        }
        else if (hero.takingDamage)
        {
            // Hero is taking damage, turn sprite red
            spriteRenderer.color = Color.red;
        }
        else
        {
            // Hero is alive and not taking damage, reset to default color
            spriteRenderer.color = Color.white;
        }
    }
}


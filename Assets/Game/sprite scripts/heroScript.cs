using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // To handle scene transitions

public class heroScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 50f; // Adjusted speed for force-based movement
    public float maxSpeed = 5f; // Maximum speed the hero can reach
    public float decelerationRate = 10f; // Rate at which the hero slows down when no input
    
    [Header("Health Setting")]    
    public float health = 100;
    
    private Vector2 moveInput;


    public heroHealthDisplayScript healthDisplay;
    public Rigidbody2D rb;

    private ghostScript ghost;
    private cyclopsScript cyclops;
    private heroEnemyScript collisionManager;
    public bool isDead = false; // Track if the hero is dead

    public bool takingDamage = false;
    private SpriteRenderer spriteRenderer; // Reference to the hero's sprite renderer

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ghost = FindObjectOfType<ghostScript>();
        cyclops = FindObjectOfType<cyclopsScript>();
        collisionManager = FindObjectOfType<heroEnemyScript>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
    }

    void Update()
    {
        if (!isDead) // Only move if not dead
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveInput = new Vector2(moveX, moveY).normalized;
        }
        Die(); // Check if hero should die
    }

    void FixedUpdate()
    {
        if (!isDead) // Only move if not dead
        {
            // Apply movement force
            rb.AddForce(moveInput * speed, ForceMode2D.Force);

            // Implement deceleration when no input is given
            if (moveInput == Vector2.zero)
            {
                // Calculate a deceleration force opposite to the current velocity
                Vector2 decelerationForce = -rb.velocity * decelerationRate;
                rb.AddForce(decelerationForce, ForceMode2D.Force);
            }

            // Limit maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        if (healthDisplay != null)
        {
            healthDisplay.Copy(health);
        }
        else
        {
            Debug.LogError("Health display reference is missing!");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        collisionManager.ManageCollisions(collision);
        takingDamage = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionManager.ManageCollisions(collision);
        takingDamage = false;
    }

    // Adjusted Die() method to stop hero movement and load GameOver after 2 seconds
    void Die()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true; // Mark hero as dead
            healthDisplay.Copy(health);
            rb.velocity = Vector2.zero; // Stop movement immediately

            StartCoroutine(TriggerGameOverScene()); // Start the coroutine to load GameOver
        }
    }

    // Coroutine to wait for 2 seconds before loading the GameOver scene
    IEnumerator TriggerGameOverScene()
    {
        yield return new WaitForSeconds(2); // Wait 2 seconds
        SceneManager.LoadScene("GameOver"); // Replace with your actual GameOver scene name
    }
}








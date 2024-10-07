using UnityEngine;

public class ghostSpawnScript : MonoBehaviour
{
    public GameObject ghostPrefab; // Prefab for the ghost
    public float spawnInterval = 5f; // Time between spawns

    // Variables that will be passed to the ghosts
    public float ghostSpeed = 3f;
    public float ghostHealth = 107f;
    public float ghostDamage = 2f;

    public eosScript enemiesOnScreen; // Reference to the enemies on screen script

    public spawnHealtScript fountainHealth;

    private float timer;

    void Start()
    {
        timer = spawnInterval; // Initialize the timer to the spawn interval
    }

    void Update()
    {
        if (fountainHealth != null && fountainHealth.Dead())
        {
            return;
        }
        
        timer -= Time.deltaTime; // Decrease the timer
        if (timer <= 0)
        {
            SpawnGhost(); // Spawn a new ghost
            timer = spawnInterval; // Reset the timer
        }
    }

    void SpawnGhost()
    {
        // Instantiate the ghost and set its initial parameters
        GameObject newGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostScript ghost = newGhost.GetComponent<ghostScript>();
        
        enemiesOnScreen.Add(); // Add the ghost to the enemies on screen

        if (ghost != null)
        {
            ghost.Initialize(ghostSpeed, ghostHealth, ghostDamage); // Pass the parameters to the ghost
            // Debug.Log("Ghost " + ghost.uniqueID + " health at spawn script: " + ghost.health);
        }
    }
}




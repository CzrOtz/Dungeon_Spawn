using UnityEngine;

public class cyclopsSpawnScript : MonoBehaviour
{
    public GameObject cyclopsPrefab;
    public float spawnInterval = 10f;
    public static int cyclopsCount;
    public eosScript enemiesOnScreen;
    private float timer;

    // Cyclops-specific properties managed by the spawner
    public float cyclopsSpeed = 3f;
    public float cyclopsHealth = 200f;
    public float cyclopsDamage = 5f;

    public spawnHealtScript fountainHealth;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        if (fountainHealth != null && fountainHealth.Dead())
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnCyclops();
            timer = spawnInterval;
            
        }
    }

    void SpawnCyclops()
    {
        // Add to the enemies on screen
        enemiesOnScreen.Add();

        // Instantiate the cyclops and pass the difficulty-related values
        GameObject newCyclops = Instantiate(cyclopsPrefab, transform.position, transform.rotation);

        cyclopsScript cyclops = newCyclops.GetComponent<cyclopsScript>();
        if (cyclops != null)
        {
            cyclops.Initialize(cyclopsSpeed, cyclopsDamage, cyclopsHealth);
            
        }
    }
}


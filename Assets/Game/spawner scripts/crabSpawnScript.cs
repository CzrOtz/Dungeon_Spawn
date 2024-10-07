using System.Collections.Generic;
using UnityEngine;

public class crabSpawnScript : MonoBehaviour
{
    public GameObject crabPrefab; // Prefab of the crab
    public float spawnInterval = 10f; // Time interval between spawns
    public float chargeInterval = 15f; // Time interval between charges for all crabs

    private float spawnTimer;
    private float chargeTimer;
    private List<crabScript> crabs = new List<crabScript>(); // Keep track of all spawned crabs

    public eosScript enemiesOnScreen;

    // Crab-specific properties managed by the spawner
    public float crabSpeed = 1f;
    public float crabHealth = 25f;
    public float crabDamage = 1f;

    public spawnHealtScript fountainHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the timers
        spawnTimer = spawnInterval;
        chargeTimer = chargeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (fountainHealth != null && fountainHealth.Dead())
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnCrab();
            spawnTimer = spawnInterval; // Reset spawn timer
        }

        // Handle charging all crabs
        chargeTimer -= Time.deltaTime;
        if (chargeTimer <= 0)
        {
            ChargeAllCrabs();
            chargeTimer = chargeInterval; // Reset charge timer
        }
    }

    // Method to spawn a new crab
    void SpawnCrab()
    {
        GameObject crabObject = Instantiate(crabPrefab, transform.position, transform.rotation);
        crabScript crab = crabObject.GetComponent<crabScript>();

        enemiesOnScreen.Add();

        if (crab != null)
        {
            crab.Initialize(crabSpeed, crabDamage, crabHealth); // Set properties from the spawner
            crabs.Add(crab); // Add spawned crab to the list
        }
    }

    // Method to make all crabs charge at the same time
    void ChargeAllCrabs()
    {
        
        foreach (crabScript crab in crabs)
        {
            if (crab != null) // Make sure the crab still exists
            {
                crab.StartCharging();
            }
        }
    }
}



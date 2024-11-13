using UnityEngine;
using System.Collections.Generic;

public class DifficultyScript : MonoBehaviour
{
    // Separate public difficulty rates as percentages (e.g., 0.1 for 10% increase per level)
    public float speedIncreaseRate = 0.1f;  // 10% increase per level
    public float healthIncreaseRate = 0.15f;  // 15% increase per level
    public float attackDamageIncreaseRate = 0.2f;  // 20% increase per level

    public float durationOfLevel = 7f; // Fast-paced level up for testing
    public timeScript timeScript; // Timer reference
    public levelScript levelScript; // Level controller reference

    private float elapsedTime = 0f; // Timer to track when to increase difficulty

    // Lists for different spawners
    [SerializeField] private List<ghostSpawnScript> ghostSpawners = new List<ghostSpawnScript>();
    [SerializeField] private List<cyclopsSpawnScript> cyclopsSpawners = new List<cyclopsSpawnScript>();
    [SerializeField] private List<crabSpawnScript> crabSpawners = new List<crabSpawnScript>();

    public winScript winScript;

    // Update is called once per frame
    void Update()
    {
        if (winScript.bossIsHere == false)
        {
                // Track elapsed time based on the game's master timer (timeScript)
            elapsedTime += Time.deltaTime;

            // Check if the elapsed time exceeds the duration of the current level
            if (elapsedTime >= durationOfLevel)
            {
                // Reset the timer for the next level
                elapsedTime = 0f;

                // Increase the level
                
            

                // Increase the difficulty for all entities
                

                if (levelScript.level <= 15)
                {
                    levelScript.IncreaseLevel();
                    IncreaseGhostDifficulty();
                    IncreaseCyclopsDifficulty();
                    IncreaseCrabDifficulty();
                }
            }
        }
 
    }

    void IncreaseGhostDifficulty()
    {
        foreach (ghostSpawnScript ghostSpawn in ghostSpawners)
        {
            if (ghostSpawn != null)
            {
                // Increase ghost properties in the spawn script as a percentage of the current values
                ghostSpawn.ghostSpeed *= (1 + speedIncreaseRate);
                ghostSpawn.ghostHealth *= (1 + healthIncreaseRate);
                ghostSpawn.ghostDamage *= (1 + attackDamageIncreaseRate);

                
            }
        }
    }

    void IncreaseCyclopsDifficulty()
    {
        foreach (cyclopsSpawnScript cyclopsSpawn in cyclopsSpawners)
        {
            if (cyclopsSpawn != null)
            {
                // Increase cyclops properties in the spawn script as a percentage of the current values
                cyclopsSpawn.cyclopsSpeed *= (1 + speedIncreaseRate);
                cyclopsSpawn.cyclopsHealth *= (1 + healthIncreaseRate);
                cyclopsSpawn.cyclopsDamage *= (1 + attackDamageIncreaseRate);

                
            }
        }
    }

    void IncreaseCrabDifficulty()
    {
        foreach (crabSpawnScript crabSpawn in crabSpawners)
        {
            if (crabSpawn != null)
            {
                // Increase crab properties in the spawn script as a percentage of the current values
                crabSpawn.crabSpeed *= (1 + speedIncreaseRate);
                crabSpawn.crabHealth *= (1 + healthIncreaseRate);
                crabSpawn.crabDamage *= (1 + attackDamageIncreaseRate);

                
            }
        }
    }
}



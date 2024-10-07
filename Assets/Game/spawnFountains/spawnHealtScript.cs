using UnityEngine;

public class spawnHealtScript : MonoBehaviour
{
    public float health = 1000f;  // Initial health of the spawn point
    private bool isDead = false;  // Track if the spawn point is dead
    private greenLiquidScript[] greenLiquidScripts;  // Reference to greenLiquidScript instances

    public winScript checkWin;  // Reference to the checkWin script

    void Start()
    {
        // Find all instances of greenLiquidScript attached to this specific spawn point
        greenLiquidScripts = GetComponentsInChildren<greenLiquidScript>(); // Now, only affects greenLiquidScripts under this spawn point's hierarchy
    }

    // Method to reduce health
    public void TakeDamage(float damage)
    {
        // Prevent further damage after health reaches 0
        if (isDead) return;

        health -= damage;

        // Clamp health to 0 and mark as dead when health reaches 0
        if (health <= 0 && !isDead)
        {
            health = 0;  // Ensure health does not go below 0
            isDead = true;  // Mark as dead
            StopGreenLiquidFlashing();  // Stop the green liquid flashing
            checkWin.deadFountains++;  // Increment the number of dead fountains
            
        }
    }

    // Method to check if the spawn point is dead
    public bool Dead()
    {
        return isDead;
    }

    // Function to stop the green liquid flashing
    private void StopGreenLiquidFlashing()
    {
        // Loop through all greenLiquidScripts attached to this spawn point and stop their flashing
        foreach (greenLiquidScript greenLiquid in greenLiquidScripts)
        {
            greenLiquid.StopFlashing();  // Stop flashing
        }
    }
}







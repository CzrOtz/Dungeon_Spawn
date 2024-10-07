using UnityEngine;
using TMPro;

public class spawnHealthTextScript : MonoBehaviour
{
    public TMP_Text healthText;  // Reference to the TMP_Text component
    public spawnHealtScript spawnHealthScript;  // Reference to the spawn point's health script (assigned per instance)
    public int misteryNumber;

    // Start is called before the first frame update
    void Start()
    {
        // Check if spawnHealthScript is assigned
        if (spawnHealthScript == null)
        {
            Debug.LogError("spawnHealthScript not assigned.");
        }

        // Check if healthText is assigned
        if (healthText == null)
        {
            Debug.LogError("Health TMP_Text not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the spawn point exists and health text is assigned
        if (spawnHealthScript != null && healthText != null)
        {
            // Display the health as an integer, clamped to 0
            healthText.text = Mathf.Max(0, (int)spawnHealthScript.health).ToString();

            if (spawnHealthScript.Dead())
            {
                // Disable the health text if the spawn point is dead
                healthText.text = "";
            }
        }
    }
}







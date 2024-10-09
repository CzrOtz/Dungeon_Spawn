using TMPro;
using UnityEngine;

public class wpnDamageScript : MonoBehaviour
{
    public TMP_Text weaponDamageText;  // Reference to the TextMeshPro UI component
    private spearSpawnerScript spearSpawner;  // Reference to the spear spawner script

    void Start()
    {
        // Find the spearSpawnerScript in the scene
        spearSpawner = FindObjectOfType<spearSpawnerScript>();

        if (spearSpawner == null)
        {
            Debug.LogError("Spear Spawner script not found!");
        }
    }

    void Update()
    {
        // If the spearSpawnerScript is found, update the weapon damage display
        if (spearSpawner != null)
        {
            // Display damage with 2 decimal places
            weaponDamageText.text = "WPN Damage: " + spearSpawner.damage.ToString("F1");
        }
    }
}



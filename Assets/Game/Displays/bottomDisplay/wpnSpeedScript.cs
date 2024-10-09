using TMPro;
using UnityEngine;

public class wpnSpeedScript : MonoBehaviour
{
    public TMP_Text weaponSpeedText;  // Reference to the TextMeshPro UI component
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
        // If the spearSpawnerScript is found, update the weapon speed display
        if (spearSpawner != null)
        {
            weaponSpeedText.text = "WPN Speed: " + spearSpawner.speed.ToString();
        }
    }
}

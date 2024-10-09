using TMPro;
using UnityEngine;

public class GhostHScript : MonoBehaviour
{
    public TMP_Text healthText;  // Reference to the TextMeshPro component
    private ghostScript ghostScript;  // Reference to the ghost script

    void Start()
    {
        // ghostScript = FindAnyObjectByType<ghostScript>();  // Find the ghost script
        if (healthText == null)
        {
            healthText = GetComponent<TMP_Text>();  // Find the TextMeshPro component

            if (healthText == null)
            {
                Debug.Log("something went wrong when refencing ch script INSIDE ch script");
            }
        }
    }

    void UpdateText(int health)
    {
        if (healthText != null && health > 0)
        {
            healthText.text = health.ToString();  
        } 
        else
        {
            healthText.text = " ";
        }
    }

    public void MirrorHealth(int health)
    {
        UpdateText(health);
        // Debug.Log("Ghost " + ghostScript.uniqueID + "health at HealthText script: " + health);
    }
}

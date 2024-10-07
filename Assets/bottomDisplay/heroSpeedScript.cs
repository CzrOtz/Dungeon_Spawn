using TMPro;
using UnityEngine;

public class heroSpeedScript : MonoBehaviour
{
    public TMP_Text heroSpeedText;  // Reference to the text component in the UI
    private heroScript hero;        // Reference to the hero script

    void Start()
    {
        // Find and reference the heroScript
        hero = FindObjectOfType<heroScript>();

        if (hero == null)
        {
            Debug.LogError("Hero script not found!");
        }
        else
        {
            // Directly display the set speed of the hero when the game starts
            heroSpeedText.text = "Speed: " + hero.speed.ToString("F2");  // Display with 2 decimal points
        }
    }

    void Update()
    {
        // If the hero script is available, ensure the speed is always displaying the set speed
        if (hero != null)
        {
            // Update the UI with the set speed, not the live velocity
            heroSpeedText.text = "Speed: " + hero.speed.ToString("F2");  // Display with 2 decimal points
        }
    }
}



using TMPro;
using UnityEngine;

public class heroTHscript : MonoBehaviour
{
    public TMP_Text healthText;  // Reference to the TextMeshPro component above the hero

    // This method is called from the heroHealthDisplayScript to update the hero's health text
    public void MirrorHealth(float health)
    {
        // Update the text with the hero's health
        if (healthText != null)
        {
            healthText.text =  health.ToString();
        }
        else
        {
            Debug.LogError("Health Text component is not assigned on heroTHscript.");
        }
    }
}

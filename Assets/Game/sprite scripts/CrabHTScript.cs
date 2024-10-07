using TMPro;
using UnityEngine;

public class CrabHTScript : MonoBehaviour
{
   public TMP_Text healthText;  // Reference to the TextMeshPro component

    void Start()
    {
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
            healthText.text = health.ToString();  // Update text with the cyclops's health
        }
        else
        {
            healthText.text = " ";
        }
    }

    public void MirrorHealth(int health)
    {
        UpdateText(health);  // Mirror the health of the cyclops
    }
}

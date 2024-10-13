using UnityEngine;
using TMPro;

public class heroHealthDisplayScript : MonoBehaviour
{
    public float heroHealth;  // The current health value to display

    public heroTHscript heroTHscript;  // Reference to the heroTHscript

    public TMP_Text heroHealthText;  // Reference to the health display text

    // Start is called before the first frame update
    void Start()
    {
        heroTHscript = FindObjectOfType<heroTHscript>();  // Find the heroTHscript in the scene

        if (heroTHscript == null)
        {
            Debug.LogError("heroTHscript not found in scene.");
        }

        UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay();
    }

    // Copy the actual health value and update the display
    public void Copy(float actualHealth)
    {
        heroHealth = actualHealth;
        if (heroTHscript != null)
        {
            heroTHscript.MirrorHealth((int)heroHealth);  // Call MirrorHealth in heroTHscript
        }
    }

    // Update the health display on the TMP_Text component
    public void UpdateHealthDisplay()
    {
        if (heroHealthText != null)
        {
            heroHealthText.text = "Health: " + heroHealth;
        }
    }
}


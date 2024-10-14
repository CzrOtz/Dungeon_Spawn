using UnityEngine;
using TMPro;

public class levelScript : MonoBehaviour
{
    public int level = 1; // Starting level
    public TMP_Text levelText; // Reference to the UI text component for displaying the level
    public float displayTime = 1.5f; // Time to display the level text

    

    void Start()
    {
        // Initialize the level display when the game starts
        UpdateLevelDisplay();
    }

    public void IncreaseLevel()
    {
        // Increment the level and update the display
        level++;
        UpdateLevelDisplay();
    }

    private void UpdateLevelDisplay()
    {
        // Ensure that the levelText is assigned and update the displayed text
        if (levelText != null)
        {
            levelText.text = "LvL: " + level.ToString();
            levelText.gameObject.SetActive(true); // Ensure the text is visible
            StartCoroutine(HideLevelTextAfterDelay());
        }
        else
        {
            Debug.LogError("Level text UI component is missing!");
        }
    }

    private System.Collections.IEnumerator HideLevelTextAfterDelay()
    {
        // Wait for 1.5 seconds before hiding the level text
        yield return new WaitForSeconds(displayTime);
        levelText.gameObject.SetActive(false); // Hide the text after the delay
    }

    public int GetCurrentLevel()
    {
        // Optionally, return the current level if needed elsewhere in the game
        return level;
    }
}


using UnityEngine;
using TMPro;

public class levelScript : MonoBehaviour
{
    public int level = 1; // Starting level
    public TMP_Text levelText; // Reference to the UI text component for displaying the level
    public float displayTime = 1.5f; // Time to display the level text

    public winScript winScript;
    

    void Start()
    {
        // Initialize the level display when the game starts
        UpdateLevelDisplay();
    }

    public void IncreaseLevel()
    {
        //boss is false when its true then we dont hwant this anymore
        if (winScript.bossIsHere == false && level <= 15)
        {
            level++;
            UpdateLevelDisplay();
        }
        
    }

    private void UpdateLevelDisplay()
    {
        // Ensure that the levelText is assigned and update the displayed text
        if (levelText != null && level <= 15)
        {
            levelText.text = "LvL: " + level.ToString();
            levelText.gameObject.SetActive(true); // Ensure the text is visible
            StartCoroutine(HideLevelTextAfterDelay());
        }
        else
        {

            levelText.text = "Max LvL Reached";
            levelText.gameObject.SetActive(true); // Hide the text if it's missing
            StartCoroutine(HideLevelTextAfterDelay());
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


using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bossCountDownScript : MonoBehaviour
{
    // public TPM_Text countdownText;  // Reference to the UI Text component
    public TMP_Text countdownText;  // Reference to the UI Text component

    void Start()
    {
        countdownText.gameObject.SetActive(false);  // Hide countdown text initially
    }

    // This method will update the countdown timer on screen
    public void ShowCountDown(int seconds)
    {
        countdownText.gameObject.SetActive(true);  // Show countdown text
        countdownText.text = "BOSS IN: " + seconds.ToString();  // Update the text with the current time
    }

    // This method will hide the countdown when the boss spawns
    public void HideCountDown()
    {
        countdownText.gameObject.SetActive(false);  // Hide countdown text
    }
}


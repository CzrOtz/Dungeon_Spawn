using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class youWinScript : MonoBehaviour
{
    public TMP_Text winMessage; // Reference to the TextMeshPro component

    void Start()
    {
        // Check if the static win variable is true
        if (winScript.won)
        {
            // Display "You Win" message if the game was won
            winMessage.text = "You Win!";
        }
        else
        {
            // Clear the message if the game wasn't won
            winMessage.text = "You died";
        }
    }
}


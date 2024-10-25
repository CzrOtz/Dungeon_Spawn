using UnityEngine;
using TMPro;

public class youWinScript : MonoBehaviour
{
    [Header("Lose Audio")]
    public AudioClip youLoseSound;  // Sound for losing the game
    public AudioSource audioSource1;  // AudioSource for losing sound

    [Header("Win Audio")]
    public AudioClip youWinSound;   // Sound for winning the game
    public AudioSource audioSource2;  // AudioSource for winning sound

    public TMP_Text winMessage;  // Reference to the TextMeshPro component

    public DataCollector1Script gameOData;  // Reference to the DataCollector1Script

    public bool win;

    void Start()
    {
        // Check if the static win variable is true
        if (winScript.won)
        {
            // Display "You Win" message if the game was won
            winMessage.text = "You Win!";

            // Assign the win sound to the AudioSource and set it to loop
            audioSource2.clip = youWinSound;
            audioSource2.loop = true;  // Enable looping for the win sound
            audioSource2.Play();       // Play the win sound
            win = true;
            gameOData.winOrLose(win);
        }
        else
        {
            // Display "You died" message if the game wasn't won
            winMessage.text = "You died";

            // Play the lose sound without looping
            audioSource1.PlayOneShot(youLoseSound);
            win = false;
            gameOData.winOrLose(win);
        }
    }
}



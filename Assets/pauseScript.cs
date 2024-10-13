using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public GameObject pauseUI;  // Reference to the pause UI GameObject
    private bool isPaused = false;  // To track if the game is paused

    // Update is called once per frame

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        // Toggle pause when Spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Only allow Exit or Main Menu return if the game is paused
        if (isPaused)
        {
            // Exit game with ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }

            // Return to main menu with M key
            if (Input.GetKeyDown(KeyCode.M))
            {
                ReturnToMainMenu();
            }
        }
    }

    // Pauses the game
    void PauseGame()
    {
        Time.timeScale = 0f;  // Stops in-game time
        pauseUI.SetActive(true);  // Display the pause UI
        PauseAllAudio();  // Pause all audio sources
        isPaused = true;
    }

    // Resumes the game
    void ResumeGame()
    {
        Time.timeScale = 1f;  // Resumes in-game time
        pauseUI.SetActive(false);  // Hide the pause UI
        ResumeAllAudio();  // Resume all audio sources
        isPaused = false;
    }

    // Exit the game
    void ExitGame()
    {
        Application.Quit();  // Quit the game (will only work in a build, not in the editor)
        Debug.Log("Game is exiting");  // This will be seen in the editor console
    }

    // Return to the main menu
    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;  // Make sure the game time is running
        SceneManager.LoadScene("MainMenu");  // Loads the scene named "MainMenu"
    }

    // Function to pause all audio sources
    void PauseAllAudio()
    {
        // Dynamically find all active AudioSources
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();  // Pause the audio source
            }
        }
    }

    // Function to resume all audio sources
    void ResumeAllAudio()
    {
        // Dynamically find all active AudioSources
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.UnPause();  // Resume the audio source
        }
    }
}


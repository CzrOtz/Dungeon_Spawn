using UnityEngine;

public class ScreenResolutionManager : MonoBehaviour
{
    void Start()
    {
        // Prevent this object from being destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Get the current screen width and height
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        // Set resolution based on screen size, with a default for larger screens
        if (screenWidth < 1920 || screenHeight < 1080) // Adjusts for smaller screens
        {
            Screen.SetResolution(screenWidth, screenHeight, false); // Sets resolution to match the screen
        }
        else
        {
            Screen.SetResolution(1920, 1080, false); // Default resolution for larger screens
        }
    }
}


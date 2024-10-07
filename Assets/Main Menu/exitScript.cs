using UnityEngine;

public class exitScript : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void ExitGame()
    {
        // Log a message to the console to confirm the button was pressed
        Debug.Log("Exit button clicked. Exiting game...");

        // If running in the editor, stop the play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Quit the application if running as a built application
            Application.Quit();
        #endif
    }
}


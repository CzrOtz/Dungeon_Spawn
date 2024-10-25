using UnityEngine;
using UnityEngine.SceneManagement; // Add this to handle scene loading

public class plyaButtonScript : MonoBehaviour
{
    // This method will be called when the Play button is clicked
    
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the name of your game scene
        
    }
}
